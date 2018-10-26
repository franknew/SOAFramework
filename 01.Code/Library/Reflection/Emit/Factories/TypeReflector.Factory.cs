using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SOAFramework.Library
{
    public partial class TypeReflector
    {
        internal static TypeReflector Create(Type typeInfo)
        {
            if (typeInfo == null)
            {
                throw new ArgumentNullException(nameof(typeInfo));
            }
            return ReflectorCacheUtils<Type, TypeReflector>.GetOrAdd(typeInfo, info => new TypeReflector(info));
        }
    }
}