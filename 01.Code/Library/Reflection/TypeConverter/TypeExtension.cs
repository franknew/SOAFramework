using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public static class TypeExtension
    {
        public static bool IsInheritFrom(this Type t, Type parentType)
        {
            return CheckType(t, parentType);
        }

        public static bool CheckType(Type son, Type parent)
        {
            if (parent is Object || parent.BaseType == null) { return false; }
            if (son.Namespace.Equals(parent.Name) && son.Namespace.Equals(parent.Name)) { return true; }
            return CheckType(son, parent.BaseType);
        }
    }
}
