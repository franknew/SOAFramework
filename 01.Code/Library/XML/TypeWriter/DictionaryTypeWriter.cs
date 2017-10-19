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
    public class DictionaryTypeWriter : ITypeWriter
    {
        public XmlWriter ObjectToXmlNode(object o, XmlWriter writer, PropertyInfo property)
        {
            var dic = o as IDictionary;
            string propertyName = property.Name;
            var attr = property.GetCustomAttributes(typeof(XmlAttributeAttribute), true)?.FirstOrDefault();
            if (attr != null) propertyName = (attr as XmlAttributeAttribute).AttributeName;
            else
            {
                var arrayItemAttr = property.GetCustomAttributes(typeof(XmlArrayItemAttribute), true)?.FirstOrDefault();
                if (arrayItemAttr != null) propertyName = (arrayItemAttr as XmlArrayItemAttribute)?.ElementName;
            }
            writer.WriteStartElement(propertyName);
            if (o != null)
            {
                foreach (var key in dic.Keys)
                {
                    var obj = dic[key];
                    Type t = obj.GetType();
                    var type = TypeUtility.CheckType(t);
                    var typeWriter = TypeWriterFactory.Create(type);
                    typeWriter.ObjectToXmlNode(obj, writer, new CustomPropertyInfo(key.ToString(), t));
                }
            }
            return writer;
        }
    }
}
