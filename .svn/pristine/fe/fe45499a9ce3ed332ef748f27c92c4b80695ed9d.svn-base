using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SOAFramework.Service.Core
{
    public static class TypeExtension
    {
        public static T GetCustomAttribute<T>(this _MemberInfo type, bool inherit)
        {
            T t = default(T);
            object[] array = type.GetCustomAttributes(typeof(T), inherit);
            if (array != null && array.Length > 0)
            {
                t = (T)array[0];
            }
            return t;
        }

        public static IEnumerable<T> GetCustomAttributes<T>(this _MemberInfo type, bool inherit)
        {
            IList<T> list = new List<T>();
            object[] array = type.GetCustomAttributes(typeof(T), inherit);
            if (array != null)
            {
                foreach (var a in array)
                {
                    list.Add((T)a);
                }
            }
            return list;
        }
    }
}
