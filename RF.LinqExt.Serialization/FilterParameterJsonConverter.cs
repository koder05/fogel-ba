using System;
using System.Linq;
using System.Text.RegularExpressions;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RF.LinqExt.Serialization
{
    internal class FilterParameterJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(FilterParameter);
        }

        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var fp = value as FilterParameter;

            if (fp != null)
            {
                writer.WriteStartObject();
                writer.WritePropertyName(string.Format("and'{0}'or'{1}'", fp.AndGroupName, fp.OrGroupName));
                writer.WriteValue(string.Format("{0} {1} {2}'{3}'", fp.ColumnName, fp.Operator, fp.Value != null ? fp.Value.GetType().Name : "", JsonConvert.SerializeObject(fp.Value)));
                writer.WriteEndObject();
            }
            else
                serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                JObject jObject = JObject.Load(reader);
                FilterParameter fp = new FilterParameter();
                Regex rx = new Regex("and'(?<and>[\\w\\d]*)'or'(?<or>[\\w\\d]*)'");
                Match m = rx.Match(jObject.Properties().ElementAt(0).Name);
                if (m != null && m.Success)
                {
                    fp.AndGroupName = m.Groups["and"].Value;
                    fp.OrGroupName = m.Groups["or"].Value;
                }

                rx = new Regex("^(?<colname>[^\\s]*)\\s(?<op>[^\\s]*)\\s(?<valtype>[\\d\\w\\.]*)'(?<valval>.*)'$");
                m = rx.Match((string)jObject.Properties().ElementAt(0).Value);

                if (m != null && m.Success)
                {
                    fp.ColumnName = m.Groups["colname"].Value;
                    fp.Operator = (OperatorType)Enum.Parse(typeof(OperatorType), m.Groups["op"].Value);
                    Type targetType = Type.GetType("System." + m.Groups["valtype"].Value);
                    object o = JsonConvert.DeserializeObject(m.Groups["valval"].Value);
                    if (o != null && targetType != null && o.GetType() != targetType)
                    {
                        if (targetType == typeof(Guid) && o.GetType() == typeof(string))
                        {
                            o = new Guid((string)o);
                        }
                        else
                        {
                            o = Convert.ChangeType(o, targetType);
                        }
                    }

                    fp.Value = o;
                }

                return fp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
