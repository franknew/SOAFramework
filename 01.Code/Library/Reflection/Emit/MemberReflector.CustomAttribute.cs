using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SOAFramework.Library
{
    public abstract partial class MemberReflector<TMemberInfo> : ICustomAttributeReflectorProvider where TMemberInfo : MemberInfo
    {
        private readonly CustomAttributeReflector[] _customAttributeReflectors;

        public CustomAttributeReflector[] CustomAttributeReflectors => _customAttributeReflectors;
    }
}