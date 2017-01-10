using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public interface ITypeConverter
    {
        object Convert(object o, Type t);
        T Convert<T>(object o);
    }

    public class TypeConverterFactory
    {
        public static ITypeConverter Create(ObjectTypeEnum type)
        {
            ITypeConverter converter = null;
            switch (type)
            {
                case ObjectTypeEnum.Value:
                    converter = new ValueTypeConverter();
                    break;
                case ObjectTypeEnum.Class:
                    converter = new ClassTypeConverter();
                    break;
                case ObjectTypeEnum.Dictionary:
                    converter = new DictionaryValueConverter();
                    break;
            }
            return converter;
        }
    }
}
