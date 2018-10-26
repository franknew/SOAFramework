using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SOAFramework.Library
{
    internal static class MethodInfoConstant
    {
        internal static readonly MethodInfo GetTypeFromHandle = InternalExtensions.GetMethod<Func<RuntimeTypeHandle, Type>>(handle => Type.GetTypeFromHandle(handle));

        internal static readonly MethodInfo GetMethodFromHandle = InternalExtensions.GetMethod<Func<RuntimeMethodHandle, RuntimeTypeHandle, MethodBase>>((h1, h2) => MethodBase.GetMethodFromHandle(h1, h2));

        internal static readonly ConstructorInfo ArgumentNullExceptionCtor = typeof(ArgumentNullException).GetConstructor(new Type[] { typeof(string) });

        internal static readonly ConstructorInfo ObjectCtor = typeof(object).GetConstructors(BindingFlags.DeclaredOnly).Single();
    }
}
