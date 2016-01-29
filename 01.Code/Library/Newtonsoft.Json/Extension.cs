using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public static class JsonExtension
    {
        public static T Clone<T>(this T t)
        {
            string json = JsonHelper.Serialize(t);
            T instance = JsonHelper.Deserialize<T>(json);
            return instance;
        }
    }
}
