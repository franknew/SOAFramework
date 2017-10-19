using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SOAFramework.Library
{
    public class ArraryOrListTypeWriter : ITypeWriter
    {
        public XmlWriter ObjectToXmlNode(object o, XmlWriter writer, PropertyInfo property)
        {
            if (o == null) return writer;
            bool pass = false;
            string propertyName = property.Name;
            var arrayAttr = property.GetCustomAttributes(typeof(XmlArrayAttribute), true)?.FirstOrDefault();
            var arrayItemAttr = property.GetCustomAttributes(typeof(XmlArrayItemAttribute), true)?.FirstOrDefault();
            string arrayItemName = null;
            if (arrayAttr != null) propertyName = (arrayAttr as XmlArrayAttribute)?.ElementName;
            if (arrayItemAttr != null) arrayItemName = (arrayItemAttr as XmlArrayItemAttribute)?.ElementName;
            var list = o as IList;
     
            pass = property.GetCustomAttributes(typeof(XmlPass), true)?.FirstOrDefault() != null;
            if (!pass) writer.WriteStartElement(propertyName);
          
            foreach (var l in list)
            {
                Type itemType = l.GetType();
                var type = TypeUtility.CheckType(itemType);
                var typewriter = TypeWriterFactory.Create(type);
                typewriter.ObjectToXmlNode(l, writer, property);
            }
            if (!pass) writer.WriteEndElement();
            return writer;
        }
    }
}
