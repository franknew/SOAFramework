using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SOAFramework.Library
{
    public class ClassTypeConverter : ITypeConverter
    {
        public object Convert(object o, Type t)
        {
            var value = Activator.CreateInstance(t);
            var properties = t.GetProperties();
            foreach (PropertyInfo property  in properties)
            {
                var obj = property.GetValue(o, null);
                if (obj == null) continue;
                value.TrySetValue(property.Name, obj);
            }
            return value;
        }

        public T Convert<T>(object o)
        {
            return (T)Convert(o, typeof(T));
        }
    }
}
