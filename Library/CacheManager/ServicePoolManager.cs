using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Reflection;

namespace SOAFramework.Library
{
    public class ServicePoolManager
    {
        public static readonly Dictionary<string, object> instance = new Dictionary<string, object>();

        public static void AddItem(string key, object value)
        {
            if (!instance.ContainsKey(key))
            {
                instance.Add(key, value);
            }
        }

        public static void SetItem(string key, object value)
        {
            if (instance.ContainsKey(key))
            {
                instance[key] = value;
            }
        }

        public static T GetItem<T>(string key) 
        {
            T t = default(T);
            if (instance.ContainsKey(key))
            {
                t = (T)instance[key];
            }
            return t;
        }

        public static List<T> GetAllItems<T>()
        {
            List<T> list = new List<T>();
            foreach (var key in instance.Keys)
            {
                T t = default(T);
                t = (T)instance[key];
                list.Add(t);
            }
            return list;
        }
    }
}
