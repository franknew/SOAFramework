using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public class ValueHelper
    {
        public static T GetValue<T>(object value)
        {
            T t = default(T);
            if (value != null)
            {
                t = (T)value.ChangeTypeTo(typeof(T));
            }
            return t;
        }
    }
}
