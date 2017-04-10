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
    public class ArrayOrListTypeReader : ITypeReader
    {
        public object XmlNodeToObject(object o, XmlNode node, PropertyInfo p)
        {
            object value = null;
            string arrName = p.Name;
            string itemName = "";
            if (p.PropertyType.IsGenericType) itemName = p.PropertyType.GetGenericArguments()[0].Name;
            else itemName = p.PropertyType.GetElementType().Name;
            var arrayAttr = p.PropertyType.GetCustomAttributes(typeof(XmlArrayAttribute), true);
            var itemAttr = p.PropertyType.GetCustomAttributes(typeof(XmlArrayItemAttribute), true);
            if (arrayAttr.Length > 0) arrName = (arrayAttr[0] as XmlArrayAttribute).ElementName;
            if (itemAttr.Length > 0) itemName = (itemAttr[0] as XmlArrayItemAttribute).ElementName;
            var arrEle = node.SelectSingleNode(arrName);
            if (arrEle == null) return value;
            var itemEle = arrEle.SelectNodes(itemName);
            if (itemEle == null) return value;
            Type elementType = null;
            int lenth = itemEle.Count;
            if (p.PropertyType.IsGenericType)
            {
                var genericTypes = p.PropertyType.GetGenericArguments();
                if (genericTypes.Length > 0)
                {
                    elementType = genericTypes[0];
                    var genericType = p.PropertyType.MakeGenericType(genericTypes[0]);
                    value = Activator.CreateInstance(genericType);
                }
            }
            else
            {
                elementType = p.PropertyType.GetElementType();
                value = Array.CreateInstance(elementType, lenth);
            }
            if (value == null) return value;
            var list = value as IList;
            p.SetValue(o, value, null);
            foreach (XmlNode n in itemEle)
            {
                var obj = Activator.CreateInstance(elementType);
                list.Add(obj);
                XmlCustomSerilizer ser = new XmlCustomSerilizer();
                ser.XmlAppendToType(obj, n, elementType);
            }
            return value;
        }
    }
}
