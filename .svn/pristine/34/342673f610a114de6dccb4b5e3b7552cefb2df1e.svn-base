using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SOAFramework.Library;

namespace MicroService.Library
{
    public class XmlSerializor : ISerializable
    {
        public object Deserialize(string xml, Type t)
        {
            return XMLHelper.Deserialize(xml, t);
        }

        public T Deserialize<T>(string xml)
        {
            return XMLHelper.Deserialize<T>(xml);
        }

        public string Serialize(object o)
        {
            return XMLHelper.Serialize(o);
        }
    }
}
