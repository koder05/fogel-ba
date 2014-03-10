using System;
using Newtonsoft.Json;

namespace RF.LinqExt.Serialization
{
    public static class JsonSerialization
    {
        public static string JsonSerialize(this FilterParameterCollection fc)
        {
            var st = new JsonSerializerSettings();
            st.Converters.Add(new FilterResolversJsonConverter());
            string s = JsonConvert.SerializeObject(fc, st);
            return s;
        }

        public static FilterParameterCollection FilterParameterCollectionJsonDeserialize(string json)
        {
            var st = new JsonSerializerSettings();
            st.Converters.Add(new FilterResolversJsonConverter());
            var fcnew = JsonConvert.DeserializeObject<FilterParameterCollection>(json, st);
            return fcnew;
        }

        public static string JsonSerialize(this SortParameterCollection sc)
        {
            var st = new JsonSerializerSettings();
            st.Converters.Add(new SortResolversJsonConverter());
            string s = JsonConvert.SerializeObject(sc, st);
            return s;
        }

        public static SortParameterCollection SortParameterCollectionJsonDeserialize(string json)
        {
            var st = new JsonSerializerSettings();
            st.Converters.Add(new SortResolversJsonConverter());
            var scnew = JsonConvert.DeserializeObject<SortParameterCollection>(json, st);
            return scnew;
        }
    }
}
