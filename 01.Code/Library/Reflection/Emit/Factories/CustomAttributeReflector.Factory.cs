using System.Reflection;

namespace SOAFramework.Library
{
    public partial class CustomAttributeReflector
    {
        internal static CustomAttributeReflector Create(CustomAttributeData customAttributeData)
        {
            return ReflectorCacheUtils<CustomAttributeData, CustomAttributeReflector>.GetOrAdd(customAttributeData, data => new CustomAttributeReflector(data));
        }
    }
}
