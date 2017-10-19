using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public class TypeUtility
    {
        public static ObjectTypeEnum CheckType(Type t)
        {
            if (t.IsValueType || t.Equals(typeof(string)))
            {
                return ObjectTypeEnum.Value;
            }
            else if (t.IsArray || t.GetInterface("IList") != null)
            {
                return ObjectTypeEnum.ArrayOrList;
            }
            else if (t.Name.ToLower().StartsWith("dictionary"))
            {
                return ObjectTypeEnum.Dictionary;
            }
            else if (t.Name.ToLower().StartsWith("datatable"))
            {
                return ObjectTypeEnum.DataTable;
            }
            else if (t.Name.ToLower().StartsWith("datarow"))
            {
                return ObjectTypeEnum.DataRow;
            }
            else if (t.Name.ToLower().StartsWith("tuple"))
            {
                throw new Exception("不支持Tuple转换");
            }
            else if (t.IsClass)
            {
                return ObjectTypeEnum.Class;
            }
            return ObjectTypeEnum.Value;
        }
    }
}
