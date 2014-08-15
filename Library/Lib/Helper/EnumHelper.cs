using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace Athena.Unitop.Sure.Lib
{
    /// <summary>
    /// 枚举值帮助类
    /// Create by CZQ
    /// </summary>
    public class EnumHelper
    {
        /// <summary>
        /// 转换成数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> EnumToList<T>()
        {
            Type enumType = typeof(T);

            // Can't use generic type constraints on value types,
            // so have to do check like this
            if (enumType.BaseType != typeof(System.Enum))
                throw new ArgumentException("T must be of type System.Enum");

            Array enumValArray = System.Enum.GetValues(enumType);
            List<T> enumValList = new List<T>(enumValArray.Length);

            foreach (int val in enumValArray)
            {
                enumValList.Add((T)System.Enum.Parse(enumType, val.ToString()));
            }

            return enumValList;
        }
        #region##获得Enum类型description
        ///<summary>
        /// 获得Enum类型description
        ///</summary>
        ///<param name="enumType">枚举的类型</param>
        ///<param name="val">枚举值</param>
        ///<returns>string</returns>
        public static string GetEnumDesc(Type enumType, object val)
        {
            string enumvalue = System.Enum.GetName(enumType, val);
            if (string.IsNullOrEmpty(enumvalue))
            {
                return Convert.ToString(val);
            }
            System.Reflection.FieldInfo finfo = enumType.GetField(enumvalue);
            object[] enumAttr = finfo.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), true);
            if (enumAttr.Length > 0)
            {
                System.ComponentModel.DescriptionAttribute desc = enumAttr[0] as System.ComponentModel.DescriptionAttribute;
                if (desc != null)
                {
                    return desc.Description;
                }
            }
            return enumvalue;
        }
        #endregion

        #region##获取某个枚举的全部信息
        ///<summary>
        /// 获取某个枚举的全部信息
        ///</summary>
        ///<typeparam name="T">枚举</typeparam>
        ///<returns>枚举的全部信息</returns>
        public static List<ReadEnum> GetEnumList<T>(bool insertEmptyItem = false)
        {
            List<ReadEnum> list = new List<ReadEnum>();
            ReadEnum re = null;
            Type type = typeof(T);
            if (insertEmptyItem)
            {
                re = new ReadEnum();
                re.Text = "";
                re.Value = -1;
                list.Add(re);
            }
            foreach (int enu in System.Enum.GetValues(typeof(T)))
            {
                re = new ReadEnum();
                re.Text = GetEnumDesc(type, enu);
                re.Value = enu;
                list.Add(re);
            }
            list.Sort((l, r) =>
            {
                int left = -1;
                int right = -1;
                if (l.Value != null && r.Value != null)
                {
                    int.TryParse(l.Value.ToString(), out left);
                    int.TryParse(r.Value.ToString(), out right);
                    return left - right;
                }
                else
                {
                    return 1;
                }
            });
            return list;
        }
        #endregion

        #region##根据值返回枚举对应的内容
        ///<summary>
        /// 根据值返回枚举对应的内容
        ///</summary>
        ///<typeparam name="T">枚举</typeparam>
        ///<param name="value">值(int)</param>
        ///<returns></returns>
        public static T GetModel<T>(int value)
        {
            T myEnum = (T)System.Enum.Parse(typeof(T), value.ToString(), true);
            return myEnum;
        }
        #endregion

        #region##根据值返回枚举对应的内容
        ///<summary>
        /// 根据值返回枚举对应的内容
        ///</summary>
        ///<typeparam name="T">枚举</typeparam>
        ///<param name="value">值(string)</param>
        ///<returns></returns>
        public static T GetModel<T>(string value)
        {
            T myEnum = (T)System.Enum.Parse(typeof(T), value, true);
            return myEnum;
        }
        #endregion
    }

    public class ReadEnum
    {
        public object Value { get; set; }
        public string Text { get; set; }

    }
}
