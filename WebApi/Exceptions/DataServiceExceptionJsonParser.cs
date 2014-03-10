using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebApi.Svc.Exceptions
{
    public static class DataServiceExceptionJsonParser
    {
        public static void Throw(Exception dsRexception)
        {
            Exception baseException = dsRexception.GetBaseException();
            var ex = ParseException(baseException.Message);
            if (ex != null)
            {
                throw ex;
            }
        }

        public static DataServiceException ParseException(string errorElement)
        {
            var st = new JsonSerializerSettings();
            st.Converters.Add(new DataServiceExceptionJsonConverter());
            return JsonConvert.DeserializeObject<DataServiceException>(errorElement, st);
        }
    }

    class DataServiceExceptionJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DataServiceException);
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
                return false;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                JObject jObject = JObject.Load(reader);
                DataServiceException result = null;
                DataServiceException internalException = null;
                string message = string.Empty;
                string stackTrace = string.Empty;

                foreach (var prop in jObject.Properties())
                {
                    switch (prop.Name.ToLower())
                    {
                        case "odata.error":
                        case "error":
                            result = DataServiceExceptionJsonParser.ParseException(prop.Value.ToString());
                            break;
                        case "innererror":
                            internalException = DataServiceExceptionJsonParser.ParseException(prop.Value.ToString());
                            break;
                        case "message":
                        case "exceptionmessage":
                            if (prop.Value.Type == JTokenType.Object)
                                message = string.Concat(message, (prop.Value as JObject).Properties().First(p => p.Name == "value").Value.ToString());
                            else
                                message = string.Concat(message, prop.Value.ToString());
                            break;
                        case "stacktrace":
                            stackTrace = prop.Value.ToString();
                            break;
                    }

                }

                if (result == null)
                {
                    result = new DataServiceException(message, stackTrace, internalException);
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
