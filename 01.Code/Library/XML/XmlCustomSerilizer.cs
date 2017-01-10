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
        public object XmlAppendToType(object o, XmlNode ele, Type type)
        {
            var properties = type.GetProperties();
            foreach (var p in properties)
            {
                var typeEnum = TypeUtility.CheckType(p.PropertyType);
                var reader = TypeReaderFactory.Create(typeEnum);
                reader.XmlNodeToObject(o, ele, p);
            }
            return o;
        }

        public string Serialize(object o, bool hasXmlHeader = false, bool formatted = false)
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
                    var type = TypeUtility.CheckType(t);
                    var typeWriter = TypeWriterFactory.Create(type);
                    typeWriter.ObjectToXmlNode(o, writer, new CustomPropertyInfo(root, t));
                    if (hasXmlHeader) writer.WriteEndDocument();
                }
                xml = Encoding.UTF8.GetString(ms.ToArray());
                if (!hasXmlHeader && xml.IndexOf("?>") > -1) xml = xml.Remove(0, xml.IndexOf("?>") + 2);
            }
            return xml;
        }

        public object Deserialize(string xml, Type type)
        {
            object o = Activator.CreateInstance(type);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlAppendToType(o, doc.ChildNodes[1], type);
            return o;
        }

        public T Deserialize<T>(string xml)
        {
            return (T)Deserialize(xml, typeof(T));
        }
    }
}