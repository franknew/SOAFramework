using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace SOAFramework.Library
{
    public interface ITypeWriter
    {
        XmlWriter ObjectToXmlNode(object o, XmlWriter writer, PropertyInfo property);
    }

    public class TypeWriterFactory
    {
        public static ITypeWriter Create(ObjectTypeEnum type)
        {
            ITypeWriter writer = null;
            switch (type)
            {
                case ObjectTypeEnum.ArrayOrList:
                    writer = new ArraryOrListTypeWriter();
                    break;
                case ObjectTypeEnum.Class:
                    writer = new ClassTypeWriter();
                    break;
                case ObjectTypeEnum.Dictionary:
                    writer = new DictionaryTypeWriter();
                    break;
                case ObjectTypeEnum.DataTable:
                    
                    break;
                case ObjectTypeEnum.Value:
                    writer = new ValueTypeWriter();
                    break;
            }
            return writer;
        }
    }
}
