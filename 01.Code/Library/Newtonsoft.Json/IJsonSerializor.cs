using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public interface IJsonSerializor
    {
        /// <summary>
        /// 把对象序列化成json
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="useDefaultJson"></param>
        /// <returns></returns>
        string Serialize(object obj, bool useDefaultJson = true);

        /// <summary>
        /// 把json反序列化成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        T Deserialize<T>(string json);

        /// <summary>
        /// 把json反序列化成对象
        /// </summary>
        /// <param name="json"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        object Deserialize(string json, Type t);
    }
}
