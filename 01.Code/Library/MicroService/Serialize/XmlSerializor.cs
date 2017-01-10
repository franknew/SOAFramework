﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using SOAFramework.Library;

namespace MicroService.Library
{
    public class XmlSerializor : ISerializable
    {
        const string xmlheader = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";

        public object Deserialize(string xml, Type t)
        {
            if (string.IsNullOrEmpty(xml)) return null;
            //if (!xml.StartsWith("<?")) xml = string.Format("{0}{1}", xmlheader, xml);
            string json = JsonHelper.XmlToJson(xml);
            return JsonHelper.Deserialize(json, t);
        }

        public T Deserialize<T>(string xml)
        {
            return (T)Deserialize(xml, typeof(T));
        }

        public string Serialize(object o)
        {
            //var json = JsonHelper.Serialize(o);
            //if (o != null)
            //{
            //    Type t = o.GetType();
            //    var xmlroot = t.GetCustomAttributes(typeof(XmlRootAttribute), true)?.FirstOrDefault(); 
            //    if (xmlroot != null)
            //    {
            //        var rootname = (xmlroot as XmlRootAttribute).ElementName;
            //        json = "{\"" + rootname + "\":" + json + "}";
            //    }
            //}
            //return JsonHelper.JsonToXml(json);
            //return XMLHelper.Serialize(o);
            XmlCustomSerilizer ser = new XmlCustomSerilizer();
            return ser.Serialize(o, true);
        }
    }
}
