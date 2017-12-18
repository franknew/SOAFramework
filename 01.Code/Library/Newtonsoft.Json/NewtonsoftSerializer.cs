using SOAFramework.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public class NewtonsoftSerializer : IJsonSerializer
    {
        public string Serialize(object obj, bool useDefaultFormat = true)
        {
            return JsonConvert.SerializeObject(obj, useDefaultFormat);
        }

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public object Deserialize(string json, Type t)
        {
            return JsonConvert.DeserializeObject(json, t);
        }
    }
}
