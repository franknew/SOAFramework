using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace SOAFramework.Library
{
    public class ClassTypeReader : ITypeReader
    {
        public object XmlNodeToObject(object o, XmlNode node, PropertyInfo p)
        {
            object value = null;
            string eleName = p.Name;
            var arr = p.GetCustomAttributes(typeof(XmlElement), true);
            if (arr.Length > 0) eleName = (arr[0] as XmlElement).Name;
            var eles = node.SelectSingleNode(eleName);
            if (eles == null) return value;
            value = Activator.CreateInstance(p.PropertyType);
            p.SetValue(o, value, null);
            XmlCustomSerilizer ser = new XmlCustomSerilizer();
            ser.XmlAppendToType(value, eles, p.PropertyType);
            return value;
        }
    }
}
