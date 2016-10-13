﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace SOAFramework.Library
{
    public class XmlCustomSerilizer
    {
        public XmlWriter TypeAppendToXml(object o, XmlWriter writer, string elementName = null)
        {
            if (o == null) return writer;
            Type t = o.GetType();
            var properties = t.GetProperties();
            foreach (var p in properties)
            {
                var ignore = p.GetCustomAttributes(typeof(XmlIgnoreAttribute), true);
                if (ignore != null && ignore.Length > 0) continue;
                var property = p.GetCustomAttributes(typeof(XmlElementAttribute), true).FirstOrDefault();
                string propertyName = p.Name;
                object propertyValue = p.GetValue(o, null);
                if (propertyValue == null) continue;
                if (property != null) propertyName = (property as XmlElementAttribute).ElementName;
                if (p.PropertyType.IsValueType || p.PropertyType.Equals(typeof(string)))
                {
                    var attr = p.GetCustomAttributes(typeof(XmlAttributeAttribute), true).FirstOrDefault();
                    string value = propertyValue == null ? "" : propertyValue.ToString();
                    value = HttpUtility.HtmlEncode(value);
                    if (attr != null)
                    {
                        var attibute = (attr as XmlAttributeAttribute);
                        propertyName = attibute.AttributeName;
                        writer.WriteAttributeString(propertyName, value);
                    }
                    else
                    {
                        writer.WriteElementString(propertyName, value);
                    }
                }
                else if (p.PropertyType.IsArray || propertyValue is IList)
                {
                    var arrayAttr = p.GetCustomAttributes(typeof(XmlArrayAttribute), true).FirstOrDefault();
                    var arrayItemAttr = p.GetCustomAttributes(typeof(XmlArrayItemAttribute), true).FirstOrDefault();
                    string arrayItemName = null;
                    if (arrayAttr != null) propertyName = (arrayAttr as XmlArrayAttribute).ElementName;
                    if (arrayItemAttr != null) arrayItemName = (arrayItemAttr as XmlArrayItemAttribute).ElementName;
                    writer.WriteStartElement(propertyName);
                    var list = propertyValue as IList;
                    foreach (var l in list)
                    {
                        Type itemType = l.GetType();
                        if (string.IsNullOrEmpty(arrayItemName)) arrayItemName = itemType.Name;
                        writer.WriteStartElement(arrayItemName);
                        TypeAppendToXml(l, writer, arrayItemName);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
                else if (p.PropertyType.IsClass || p.PropertyType.IsInterface)
                {
                    if (!string.IsNullOrEmpty(elementName)) propertyName = elementName;
                    writer.WriteStartElement(propertyName);
                    TypeAppendToXml(propertyValue, writer);
                    writer.WriteEndElement();
                }
            }
            return writer;
        }

        public string Serial(object o, bool hasXmlHeader = false, bool formatted = false)
        {
            string xml = null;
            using (MemoryStream ms = new MemoryStream())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                //要求缩进
                settings.Indent = formatted;
                //注意如果不设置encoding默认将输出utf-16
                //注意这儿不能直接用Encoding.UTF8如果用Encoding.UTF8将在输出文本的最前面添加4个字节的非xml内容
                settings.Encoding = new UTF8Encoding(false);
                using (XmlWriter writer = XmlWriter.Create(ms, settings))
                {
                    if (hasXmlHeader) writer.WriteStartDocument();
                    var t = o.GetType();
                    var rootAttr = t.GetCustomAttributes(typeof(XmlRootAttribute), true).FirstOrDefault();
                    string root = t.Name;
                    if (rootAttr != null) root = (rootAttr as XmlRootAttribute).ElementName;
                    if (root.Contains("`")) root = root.Substring(0, root.IndexOf("`"));
                    writer.WriteStartElement(root);
                    TypeAppendToXml(o, writer);
                    writer.WriteEndElement();
                    if (hasXmlHeader) writer.WriteEndDocument();
                }
                xml = Encoding.UTF8.GetString(ms.ToArray());
            }
            return xml;
        }
    }
}
