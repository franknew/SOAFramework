using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SOAFramework.Library;

namespace MicroService.Library
{
    public interface ISerializable
    {
        string Serialize(object o);
        T Deserialize<T>(string json);
        object Deserialize(string json, Type t);
    }

    public class SerializeFactory
    {
        public static ISerializable Create(ContentTypeEnum dataType = ContentTypeEnum.Json)
        {
            switch (dataType)
            {
                case ContentTypeEnum.Json:
                    return new JsonSerializor();
                case ContentTypeEnum.Xml:
                    return new XmlSerializor();
                case ContentTypeEnum.UrlEncoded:
                    return new UrlSerializor();
                default:
                     return null;
            }
        }
    }
}
