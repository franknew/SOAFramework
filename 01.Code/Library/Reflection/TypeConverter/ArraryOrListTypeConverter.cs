using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public class ArraryOrListTypeConverter : ITypeConverter
    {
        public object Convert(object o, Type t)
        {
            object resultList = null; ;
            Type elementType = null;
            if (t.IsArray)
            {
                elementType = t.GetElementType();
                resultList = Activator.CreateInstance(t);
            }
            else if (t.IsGenericType)
            {
                elementType = t.GetGenericArguments()[0];
                var createType = t.MakeGenericType(elementType);
                resultList = Activator.CreateInstance(createType);
            }
            else if (t is ICollection) throw new Exception("不支持Collection");
            else throw new Exception("没有匹配的数组类型");
            var oArray = o as IList;
            var list = resultList as IList;
            foreach (var e in oArray)
            {
                var data = e.ConvertTo(elementType);
                list.Add(data);
            }
            return resultList;
        }

        public T Convert<T>(object o) 
        {

            return (T)Convert(o, typeof(T));
        }
    }
}
