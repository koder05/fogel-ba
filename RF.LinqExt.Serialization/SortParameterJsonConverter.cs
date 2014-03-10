using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.ComponentModel;

using RF.LinqExt;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RF.LinqExt.Serialization
{
    class SortParameterJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(SortParameter);
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
            var sp = value as SortParameter;

            if (sp != null)
            {
                writer.WriteStartObject();
                writer.WritePropertyName(sp.ColumnName);
                writer.WriteValue(sp.SortDirection.ToString());
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
                SortParameter sp = new SortParameter(jObject.Properties().ElementAt(0).Name
                    , (ListSortDirection)Enum.Parse(typeof(ListSortDirection), (string)jObject.Properties().ElementAt(0).Value));
                return sp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
