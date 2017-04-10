using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace SOAFramework.Library
{
    public class ValueTypeWriter : ITypeWriter
    {
        public XmlWriter ObjectToXmlNode(object o, XmlWriter writer, PropertyInfo property)
        {
            var attr = property.GetCustomAttributes(typeof(XmlAttributeAttribute), true)?.FirstOrDefault();
            string value = null;
            if (o != null) value = HttpUtility.HtmlEncode(o.ToString());
            string propertyName = property.Name;
            if (attr != null)
            {
                var attibute = (attr as XmlAttributeAttribute);
                propertyName = attibute.AttributeName;
            }
            writer.WriteElementString(propertyName, value);
            return writer;
        }
    }
}
