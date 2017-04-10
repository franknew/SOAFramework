using System;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace SOAFramework.Library
{
    public class XMLHelper
    {
        /// <summary>
        /// 对象序列化成 XML String
        /// </summary>
        public static string Serialize(object obj)
        {
            string xmlString = string.Empty;
            //XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    xmlSerializer.Serialize(ms, obj);
            //    xmlString = Encoding.UTF8.GetString(ms.ToArray());
            //}
            XmlCustomSerilizer serial = new XmlCustomSerilizer();
            xmlString = serial.Serialize(obj);
            return xmlString;
        }

        /// <summary>
        /// XML String 反序列化成对象
        /// </summary>
        public static T Deserialize<T>(string xmlString)
        {
            return (T)Deserialize(xmlString, typeof(T));
        }

        public static object Deserialize(string xmlString, Type t)
        {
            //XmlCustomSerilizer ser = new XmlCustomSerilizer();
            XmlSerializer ser = new XmlSerializer(t);
            MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString));
            return ser.Deserialize(memStream);
        }

        /// <summary>
        /// XML String 反序列化成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        public static T DeserializeFromFile<T>(string xmlPath)
        {
            string xml = File.ReadAllText(xmlPath);
            return Deserialize<T>(xml);
        }

        public static string ToSOAPString(object o)
        {
            string soapString = null;
            XmlTypeMapping myTypeMapping = (new SoapReflectionImporter().ImportTypeMapping(o.GetType()));
            XmlSerializer mySerializer = new XmlSerializer(myTypeMapping);
            using (MemoryStream ms = new MemoryStream())
            {
                mySerializer.Serialize(ms, o);
                soapString = Encoding.UTF8.GetString(ms.ToArray());
            }
            return soapString;
        }
    }
}
