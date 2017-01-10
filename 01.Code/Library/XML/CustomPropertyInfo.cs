using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SOAFramework.Library
{
    public class CustomPropertyInfo : PropertyInfo
    {
        private string _name;
        private Type _type;
        public CustomPropertyInfo(string name, Type type)
        {
            _name = name;
            _type = type;
        }

        public override PropertyAttributes Attributes
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        public override Type DeclaringType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string Name
        {
            get
            {
                return _name;
            }
        }

        public override Type PropertyType
        {
            get
            {
                return _type;
            }
        }

        public override Type ReflectedType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override MethodInfo[] GetAccessors(bool nonPublic)
        {
            throw new NotImplementedException();
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            return null;
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return null;
        }

        public override MethodInfo GetGetMethod(bool nonPublic)
        {
            throw new NotImplementedException();
        }

        public override ParameterInfo[] GetIndexParameters()
        {
            throw new NotImplementedException();
        }

        public override MethodInfo GetSetMethod(bool nonPublic)
        {
            throw new NotImplementedException();
        }

        public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
