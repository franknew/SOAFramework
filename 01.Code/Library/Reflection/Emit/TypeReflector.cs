using System;
using System.Reflection;

namespace SOAFramework.Library
{
    public partial class TypeReflector : MemberReflector<Type>
    {
        private readonly string _displayName;
        private readonly string _fullDisplayName;

        private TypeReflector(Type typeInfo) : base(typeInfo)
        {
            _displayName = GetDisplayName(typeInfo);
            _fullDisplayName = GetFullDisplayName(typeInfo);
        }

        public override string DisplayName => _displayName;

        public virtual string FullDisplayName => _fullDisplayName;

        private static string GetDisplayName(Type typeInfo)
        {
            var name = typeInfo.Name.Replace('+', '.');
            if (typeInfo.IsGenericParameter)
            {
                return name;
            }
            if (typeInfo.IsGenericType)
            {
                var arguments = typeInfo.IsGenericTypeDefinition
                 ? typeInfo.GetGenericArguments()
                 : typeInfo.GetGenericArguments();
                name = name.Replace("`", "").Replace(arguments.Length.ToString(), "");
                name += $"<{GetDisplayName(arguments[0])}";
                for (var i = 1; i < arguments.Length; i++)
                {
                    name = name + "," + GetDisplayName(arguments[i]);
                }
                name += ">";
            }
            if (!typeInfo.IsNested)
                return name;
            return $"{GetDisplayName(typeInfo.DeclaringType)}.{name}";
        }

        private static string GetFullDisplayName(Type typeInfo)
        {
            var name = typeInfo.Name.Replace('+', '.');
            if (typeInfo.IsGenericParameter)
            {
                return name;
            }
            if (!typeInfo.IsNested)
            {
                name = $"{typeInfo.Namespace}." + name;
            }
            else
            {
                name= $"{GetFullDisplayName(typeInfo.DeclaringType)}.{name}";
            }          
            if (typeInfo.IsGenericType)
            {
                var arguments = typeInfo.IsGenericTypeDefinition
                 ? typeInfo.GetGenericArguments()
                 : typeInfo.GetGenericArguments();
                name = name.Replace("`", "").Replace(arguments.Length.ToString(), "");
                name += $"<{GetFullDisplayName(arguments[0])}";
                for (var i = 1; i < arguments.Length; i++)
                {
                    name += "," + GetFullDisplayName(arguments[i]);
                }
                name += ">";
            }
            return name;
        }
    }
}