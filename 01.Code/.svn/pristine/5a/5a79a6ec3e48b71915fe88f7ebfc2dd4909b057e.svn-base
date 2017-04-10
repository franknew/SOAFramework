using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SOAFramework.Library;

namespace MicroService.Library
{
    public class JsonSerializor : ISerializable
    {
        public object Deserialize(string json, Type t)
        {
            return JsonHelper.Deserialize(json, t);
        }

        public T Deserialize<T>(string json)
        {
            return JsonHelper.Deserialize<T>(json);
        }

        public string Serialize(object o)
        {
            return JsonHelper.Serialize(o);
        }
    }
}
