using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SOAFramework.Library
{
    public static class TypeExtensions
    {
        private static readonly ConcurrentDictionary<Type, bool> isTaskOfTCache = new ConcurrentDictionary<Type, bool>();
        private static readonly ConcurrentDictionary<Type, bool> isValueTaskOfTCache = new ConcurrentDictionary<Type, bool>();

        public static MethodInfo GetMethodBySignature(this Type typeInfo, MethodSignature signature)
        {
            if (typeInfo == null)
            {
                throw new ArgumentNullException(nameof(typeInfo));
            }
            return typeInfo.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).
                FirstOrDefault(m => new MethodSignature(m) == signature);
        }

        public static MethodInfo GetDeclaredMethodBySignature(this Type typeInfo, MethodSignature signature)
        {
            if (typeInfo == null)
            {
                throw new ArgumentNullException(nameof(typeInfo));
            }
            return typeInfo.GetMethods(BindingFlags.DeclaredOnly).FirstOrDefault(m => new MethodSignature(m) == signature);
        }

        public static object GetDefaultValue(this Type typeInfo)
        {
            if (typeInfo == null)
            {
                throw new ArgumentNullException(nameof(typeInfo));
            }

            if (typeInfo == typeof(void))
            {
                return null;
            }

            switch (Type.GetTypeCode(typeInfo))
            {
                case TypeCode.Object:
                case TypeCode.DateTime:
                    if (typeInfo.IsValueType)
                    {
                        return Activator.CreateInstance(typeInfo);
                    }
                    else
                    {
                        return null;
                    }

                case TypeCode.Empty:
                case TypeCode.String:
                    return null;

                case TypeCode.Boolean:
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                    return 0;

                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return 0;

                case TypeCode.Single:
                    return default(Single);

                case TypeCode.Double:
                    return default(Double);

                case TypeCode.Decimal:
                    return new Decimal(0);

                default:
                    throw new InvalidOperationException("Code supposed to be unreachable.");
            }
        }

        public static bool IsVisible(this Type typeInfo)
        {
            if (typeInfo == null)
            {
                throw new ArgumentNullException(nameof(typeInfo));
            }
            if (typeInfo.IsNested)
            {
                if (!typeInfo.DeclaringType.IsVisible())
                {
                    return false;
                }
                if (!typeInfo.IsVisible || !typeInfo.IsNestedPublic)
                {
                    return false;
                }
            }
            else
            {
                if (!typeInfo.IsVisible || !typeInfo.IsPublic)
                {
                    return false;
                }
            }
            if (typeInfo.IsGenericType && !typeInfo.IsGenericTypeDefinition)
            {
                foreach (var argument in typeInfo.GetGenericArguments())
                {
                    if (!argument.IsVisible())
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool IsTask(this Type typeInfo)
        {
            if (typeInfo == null)
            {
                throw new ArgumentNullException(nameof(typeInfo));
            }
            return typeInfo == typeof(Task);
        }

        public static bool IsTaskWithResult(this Type typeInfo)
        {
            if (typeInfo == null)
            {
                throw new ArgumentNullException(nameof(typeInfo));
            }
            return isTaskOfTCache.GetOrAdd(typeInfo, Info => Info.IsGenericType && typeof(Task).IsAssignableFrom(Info));
        }

        //public static bool IsValueTask(this Type typeInfo)
        //{
        //    if (typeInfo == null)
        //    {
        //        throw new ArgumentNullException(nameof(typeInfo));
        //    }
        //    return isValueTaskOfTCache.GetOrAdd(typeInfo, Info => Info.IsGenericType && Info.GetGenericTypeDefinition() == typeof(ValueTask<>));
        //}
    }
}