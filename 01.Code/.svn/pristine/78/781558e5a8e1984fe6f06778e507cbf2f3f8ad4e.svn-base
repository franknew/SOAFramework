using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace SOAFramework.Library
{
    public class ValueTypeReader : ITypeReader
    {
        public object XmlNodeToObject(object o, XmlNode node, PropertyInfo p)
        {
            object value = null;
            if (o == null) return value;
            Type t = o.GetType();
            string eleName = p.Name;
            var arr = p.GetCustomAttributes(typeof(XmlElement), true);
            if (arr.Length > 0) eleName = (arr[0] as XmlElement).Name;
            var eles = node.SelectSingleNode(eleName);
            if (eles != null) value = eles.InnerText;
            if (value == null) return value;
            value = Convert.ChangeType(value, p.PropertyType);
            p.SetValue(o, value, null);
            return value;
        }
    }
}
