using System;
using System.Reflection;

namespace SOAFramework.Library
{
    public partial class FieldReflector
    {
        internal static FieldReflector Create(FieldInfo reflectionInfo)
        {
            if (reflectionInfo == null)
            {
                throw new ArgumentNullException(nameof(reflectionInfo));
            }

            return ReflectorCacheUtils<FieldInfo, FieldReflector>.GetOrAdd(reflectionInfo, CreateInternal);

            FieldReflector CreateInternal(FieldInfo field)
            {
                if (field.DeclaringType.ContainsGenericParameters)
                {
                    return new OpenGenericFieldReflector(field);
                }

                if (field.DeclaringType.IsEnum)
                {
                    return new EnumFieldReflector(field);
                }

                if (field.IsStatic)
                {
                    return new StaticFieldReflector(field);
                }

                return new FieldReflector(field);
            }
        }
    }
}
