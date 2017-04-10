using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SOAFramework.Library
{
    public class DictionaryValueConverter : ITypeConverter
    {
        public object Convert(object o, Type t)
        {
            var value = Activator.CreateInstance(t);
            var dic = o as IDictionary;
            foreach (var key in dic.Keys)
            {
                object obj = dic[key];
                if (obj == null) continue;
                value.TrySetValue(key.ToString(), obj);
            }
            return value;
        }

        public T Convert<T>(object o)
        {
            return (T)Convert(o, typeof(T));
        }
    }
}
