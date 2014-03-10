using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RF.LinqExt.Serialization
{
    internal class SortResolversJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(SortParameterCollection);
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
            var fpc = value as SortParameterCollection;

            if (fpc != null)
            {
                writer.WriteStartObject();
                if (fpc.PropertyNameResolver != null)
                {
                    writer.WritePropertyName(typeof(IFilterSortPropResolver).Name);
                    writer.WriteValue(fpc.PropertyNameResolver.GetType().AssemblyQualifiedName);
                }
                if (fpc.DefaultOrder != null)
                {
                    writer.WritePropertyName("default");
                    writer.WriteValue(fpc.DefaultOrder.JsonSerialize());
                }
                writer.WritePropertyName("params");
                var st = new JsonSerializerSettings();
                st.Converters.Add(new SortParameterJsonConverter());
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
                SortParameterCollection result = null;
                IFilterSortPropResolver props = null;
                SortParameterCollection defaultSort = null;

                foreach (var prop in jObject.Properties())
                {
                    if (prop.Name == "params")
                    {
                        var st = new JsonSerializerSettings();
                        st.Converters.Add(new SortParameterJsonConverter());
                        result = JsonConvert.DeserializeObject<SortParameterCollection>((string)prop.Value, st);
                    }
                    else if (prop.Name == typeof(IFilterSortPropResolver).Name)
                    {
                        Type propsType = Type.GetType((string)prop.Value);
                        props = Activator.CreateInstance(propsType) as IFilterSortPropResolver;
                    }
                    else if (prop.Name == "default")
                    {
                        defaultSort = JsonSerialization.SortParameterCollectionJsonDeserialize((string)prop.Value);
                    }
                }

                if (result != null)
                {
                    if (props != null)
                        result.PropertyNameResolver = props;
                    if (defaultSort != null)
                        result.DefaultOrder = defaultSort;
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

