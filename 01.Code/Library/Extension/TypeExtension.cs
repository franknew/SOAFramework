using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace SOAFramework.Library
{
    public static class TypeExtension
    {
        public static T GetCustomAttribute<T>(this Type type, bool inherit = true) 
        {
            T t = default(T);
            var attrs = type.GetCustomAttributes(typeof(T), inherit);
            if (attrs.Length > 0) t = (T)attrs[0];
            return t;
        }

        public static T GetCustomAttribute<T>(this _MemberInfo member, bool inherit = true) 
        {
            T t = default(T);
            var attrs = member.GetCustomAttributes(typeof(T), inherit);
            if (attrs.Length > 0) t = (T)attrs[0];
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
