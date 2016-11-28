using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace MicroService.Library
{
    public class UrlSerializor : ISerializable
    {
        public object Deserialize(string json, Type t)
        {
            throw new NotImplementedException();
        }

        public T Deserialize<T>(string json)
        {
            throw new NotImplementedException();
        }

        public string Serialize(object o)
        {
            throw new NotImplementedException();
        }
    }
}
