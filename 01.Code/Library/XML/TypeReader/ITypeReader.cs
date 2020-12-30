﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace SOAFramework.Library
{
    public interface ITypeReader
    {
        object XmlNodeToObject(object o, XmlNode node, PropertyInfo p);
    }

    public class TypeReaderFactory
    {
        public static ITypeReader Create(ObjectTypeEnum type)
        {
            switch (type)
            {
                case ObjectTypeEnum.Value:
                    return new ValueTypeReader();
                case ObjectTypeEnum.Class:
                    return new ClassTypeReader();
                case ObjectTypeEnum.ArrayOrList:
                    return new ArrayOrListTypeReader();
            }
            return new ValueTypeReader();
        }
    }

}