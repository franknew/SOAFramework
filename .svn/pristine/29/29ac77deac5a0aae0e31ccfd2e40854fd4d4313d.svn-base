using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace SOAFramework.Common
{
    public class JSONUtil
    {
        /// <summary>
        /// 把对象序列化成JSON格式字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string JSONSerialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 把JSON格式字符串反序列化成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="JSON"></param>
        /// <returns></returns>
        public static T JSONDeserialize<T>(string JSON)
        {
            return JsonConvert.DeserializeObject<T>(JSON);
        }
    }
}
