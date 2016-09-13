using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public static ISerializable Create(DataSerializeTypeEnum dataType = DataSerializeTypeEnum.Json)
        {
            switch (dataType)
            {
                case DataSerializeTypeEnum.Json:
                    return new JsonSerializor();
                default:
                    return new JsonSerializor();
            }
        }
    }

    public enum DataSerializeTypeEnum
    {
        Json,
        Xml
    }
}
