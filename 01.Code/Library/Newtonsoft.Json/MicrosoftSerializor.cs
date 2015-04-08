using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace SOAFramework.Library
{
    public class MicrosoftSerializor : IJsonSerializor
    {
        /// <summary>
        /// 将对象序列化成json
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="useDefaultJson">是否使用自定义格式的json</param>
        /// <returns></returns>
        public string Serialize(object obj, bool useDefaultJson = true)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                StringBuilder sb = new StringBuilder();
                sb.Append(Encoding.UTF8.GetString(ms.ToArray()));
                return sb.ToString();
            } 
        }

        /// <summary>
        /// 将json反序列化成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public T Deserialize<T>(string json)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            T jsonObject = (T)ser.ReadObject(ms);
            ms.Close();
            return jsonObject; 
        }

        /// <summary>
        /// 将json反序列化成对象
        /// </summary>
        /// <param name="json"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public object Deserialize(string json, Type t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(t);
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            object jsonObject = ser.ReadObject(ms);
            ms.Close();
            return jsonObject; 
        }
    }
}
