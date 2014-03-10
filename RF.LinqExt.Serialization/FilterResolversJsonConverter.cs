using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RF.LinqExt.Serialization
{
    internal class FilterResolversJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(FilterParameterCollection);
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
            var fpc = value as FilterParameterCollection;

            if (fpc != null)
            {
                writer.WriteStartObject();
                if (fpc.OperatorActionResolver.GetType() != typeof(DefaultFilterOperatorResolver))
                {
                    writer.WritePropertyName(typeof(IFilterOperatorResolver).Name);
                    writer.WriteValue(fpc.OperatorActionResolver.GetType().AssemblyQualifiedName);
                }
                if (fpc.PropertyNameResolver != null)
                {
                    writer.WritePropertyName(typeof(IFilterSortPropResolver).Name);
                    writer.WriteValue(fpc.PropertyNameResolver.GetType().AssemblyQualifiedName);
                }
                writer.WritePropertyName("params");
                var st = new JsonSerializerSettings();
                st.Converters.Add(new FilterParameterJsonConverter());
                writer.WriteValue(JsonConvert.SerializeObject(fpc, st));
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
                FilterParameterCollection result = null;
                IFilterOperatorResolver opres = null;
                IFilterSortPropResolver props = null;

                foreach (var prop in jObject.Properties())
                {
                    if (prop.Name == "params")
                    {
                        var st = new JsonSerializerSettings();
                        st.Converters.Add(new FilterParameterJsonConverter());
                        result = JsonConvert.DeserializeObject<FilterParameterCollection>((string)prop.Value, st);
                    }
                    else if (prop.Name == typeof(IFilterOperatorResolver).Name)
                    {
                        Type opresType = Type.GetType((string)prop.Value);
                        opres = Activator.CreateInstance(opresType) as IFilterOperatorResolver;
                    }
                    else if (prop.Name == typeof(IFilterSortPropResolver).Name)
                    {
                        Type propsType = Type.GetType((string)prop.Value);
                        props = Activator.CreateInstance(propsType) as IFilterSortPropResolver;
                    }
                }

                if (result != null)
                {
                    if (opres != null)
                        result.OperatorActionResolver = opres;
                    if (props != null)
                        result.PropertyNameResolver = props;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
