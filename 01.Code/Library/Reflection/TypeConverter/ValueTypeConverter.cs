﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public class ValueTypeConverter : ITypeConverter
    {
        public object Convert(object o, Type t)
        {
            object safeValue = o;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                NullableConverter nullableConverter = new NullableConverter(t);
                t = nullableConverter.UnderlyingType;
            }
            if (!(t.Equals(typeof(Object)))) safeValue = (o == null || o == DBNull.Value) ? o
                                                           : System.Convert.ChangeType(o, t);
            return safeValue;
        }

        public T Convert<T>(object o)
        {
            return (T)Convert(o, typeof(T));
        }
    }
}
