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
        private readonly static object syncRoot = new Object();
        private static Dictionary<string, object> instance = new Dictionary<string, object>();

        /// <summary>
        /// 线程安全的单例
        /// </summary>
        public static Dictionary<string, object> Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new Dictionary<string, object>();
                        }
                    }
                }
                return instance;
            }
        }

        public static void AddItem(string key, object value)
        {
            if (!Instance.ContainsKey(key))
            {
                Instance.Add(key, value);
            }
        }

        public static void SetItem(string key, object value)
        {
            if (Instance.ContainsKey(key))
            {
                Instance[key] = value;
            }
        }

        public static T GetItem<T>(string key) 
        {
            T t = default(T);
            if (Instance.ContainsKey(key))
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
                t = (T)Instance[key];
                list.Add(t);
            }
            return list;
        }
    }
}
