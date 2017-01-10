﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SOAFramework.Library
{
    public class ClassTypeWriter : ITypeWriter
    {
        public XmlWriter ObjectToXmlNode(object o, XmlWriter writer, PropertyInfo property)
        {
            string propertyName = property.Name;
            bool pass = false;
            var attr = property.GetCustomAttributes(typeof(XmlAttributeAttribute), true)?.FirstOrDefault();
            if (attr != null) propertyName = (attr as XmlAttributeAttribute).AttributeName;
            else
            {
                var arrayItemAttr = property.GetCustomAttributes(typeof(XmlArrayItemAttribute), true)?.FirstOrDefault();
                if (arrayItemAttr != null) propertyName = (arrayItemAttr as XmlArrayItemAttribute)?.ElementName;
            }
            if (o == null) return writer;
            var t = o.GetType();
            pass = t.GetCustomAttributes(typeof(XmlPass), true)?.FirstOrDefault() != null;
            if (!pass) writer.WriteStartElement(propertyName);
            var properties = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var p in properties)
            {
                var pObj = p.GetValue(o, null);
                var type = TypeUtility.CheckType(p.PropertyType);
                var typeWriter = TypeWriterFactory.Create(type);
                typeWriter.ObjectToXmlNode(pObj, writer, p);
            }
            if (!pass) writer.WriteEndElement();
            return writer;
        }
    }
}
