﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SOAFramework.Library
{
    public static class ReflectionExtension
    {
        /// <summary>
        /// 根据属性名反射获得值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static T GetValue<T>(this object obj, string propertyName, bool isTry = false, bool newProperty = true)
        {
            return (T)obj.GetValue(propertyName, isTry, newProperty);
        }

        /// <summary>
        /// 根据属性名反射获得值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetValue(this object obj, string propertyName, bool isTry = false, bool newProperty = true)
        {
            Type type = obj.GetType();
            string[] propertyarr = propertyName.Split('.');
            object value = null;
            PropertyObject property = new PropertyObject { CurrentObject = obj };
            for (int i = 0; i < propertyarr.Length; i++)
            {
                property = GetProperty(property.CurrentObject, propertyarr[i], newProperty, isTry);
                if (property == null)
                {
                    if (isTry) return null;
                    else throw new Exception(string.Format("属性：{0}在对象：{0}中找不到！", propertyarr[i], type.FullName));
                }
            }
            value = property?.Value;
            return value;
        }

        public static object TryGetValue(this object obj, string propertyName, bool newProperty = true)
        {
            return obj.GetValue(propertyName, true, newProperty);
        }

        public static T TryGetValue<T>(this object obj, string propertyName, bool newProperty = true)
        {
            return obj.GetValue<T>(propertyName, true);
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void SetValue(this object obj, string propertyName, object value, bool isTry = false)
        {
            Type type = obj.GetType();
            string[] propertyarr = propertyName.Split('.');
            PropertyObject property = new PropertyObject { CurrentObject = obj };
            if (propertyarr.Length > 1)
            {
                for (int i = 0; i < propertyarr.Length; i++)
                {
                    property = GetProperty(property.CurrentObject, propertyarr[i], true);
                    if (property == null && !isTry) throw new Exception(string.Format("属性：{0}在对象：{0}中找不到！", propertyarr[i], type.FullName));
                }
            }
            var pro = property.CurrentObject.GetType().GetProperty(propertyarr[propertyarr.Length - 1]);
            if (pro != null)
            {
                var classType = TypeUtility.CheckType(pro.PropertyType);
                var converter = TypeConverterFactory.Create(classType);
                var safeValue = converter.Convert(value, pro.PropertyType);
                try
                {
                    pro.SetValue(property.CurrentObject, safeValue, null);
                }
                catch
                { }
            }
        }

        public static void TrySetValue(this object obj, string propertyName, object value)
        {
            obj.SetValue(propertyName, value, true);
        }

        public static PropertyObject GetProperty(object obj, string propertyName, bool newproperty = false, bool isTry = false)
        {
            if (obj == null) return null;
            Type type = obj.GetType();

            int index = 0;
            //处理数组
            if (propertyName.EndsWith("]"))
            {
                propertyName = propertyName.TrimEnd(']');
                var arr = propertyName.Split('[');
                if (arr.Length == 2) index = Convert.ToInt32(arr[1]);
                propertyName = arr[0];
            }

            var property = type.GetProperty(propertyName);
            if (property == null) return null;
            object value = property.GetValue(obj, null);

            if ((property.PropertyType.IsGenericType &&
                (property.PropertyType.GetGenericTypeDefinition() == typeof(IList<>) || property.PropertyType.GetGenericTypeDefinition() == typeof(List<>)))
                || property.PropertyType.IsArray)
            {
                #region list
                Type valueType = null;
                var valuelist = value as IList;
                if (property.PropertyType.IsGenericType)
                {
                    var args = property.PropertyType.GetGenericArguments();
                    valueType = args[0];
                }
                else valueType = property.PropertyType.GetElementType();
                if (value == null && newproperty)
                {
                    var constructedListType = property.PropertyType.GetGenericTypeDefinition().MakeGenericType(valueType);
                    var instance = Activator.CreateInstance(constructedListType);
                    value = Activator.CreateInstance(valueType);
                    valuelist = instance as IList;
                    valuelist.Add(value);
                    object listvalue = valuelist;
                    if (property.PropertyType.IsArray)
                    {
                    }
                    property.SetValue(obj, listvalue, null);
                }
                else if (value == null) return null;
                else
                {
                    if (index >= valuelist.Count) throw new IndexOutOfRangeException();
                    else value = valuelist[index];
                }
                property = valueType.GetProperty(propertyName);
                #endregion
            }
            else if (property.PropertyType == typeof(DataTable))
            {
                #region datatable
                DataTable table = new DataTable();
                if (value == null && newproperty)
                {
                    value = table;
                    DataRow row = table.NewRow();
                    table.Rows.Add(row);
                    property.SetValue(obj, value, null);
                    value = row;
                }
                else
                {
                    table = value as DataTable;
                    if (index >= table.Rows.Count) throw new IndexOutOfRangeException(); 
                    else value = table.Rows[index]; 
                }
                #endregion
            }
            else if (property.PropertyType == typeof(DataRow))
            {
                #region datarow
                DataTable table = obj as DataTable;
                if (value == null && newproperty)
                {
                    DataRow row = table.NewRow();
                    table.Rows.Add(row);
                    value = row;
                }
                DataRow valuerow = value as DataRow;
                if (!table.Columns.Contains(property.Name))
                {
                    DataColumn column = new DataColumn(property.Name);
                    table.Columns.Add(column);
                }
                value = valuerow[property.Name];
                #endregion
            }
            else
            {
                #region object
                if (value == null && newproperty && property.PropertyType.GetConstructors().Any(t => t.GetParameters().Length == 0))
                {
                    value = Activator.CreateInstance(property.PropertyType);
                    property.SetValue(obj, value, null);
                }
                #endregion
            }
            return new PropertyObject { CurrentObject = value, Value = value, Property = property };
        }

        /// <summary>
        /// 对象是否有某属性
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool HasProperty(this object obj, string propertyName)
        {
            return GetProperty(obj, propertyName) != null;
        }

        public static T ToObject<T>(this IDictionary dic, T obj = default(T))
        {
            if (obj == null) obj = Activator.CreateInstance<T>();
            foreach (string key in dic.Keys)
            {
                obj.TrySetValue(key, dic[key]);
            }
            return obj;
        }

        public static T ConvertTo<T>(this object o)
        {
            return (T)o.ConvertTo(typeof(T));
        }

        public static object ConvertTo(this object o, Type t)
        {
            var type = TypeUtility.CheckType(t);
            var converter = TypeConverterFactory.Create(type);
            return converter.Convert(o, t);
        }

        public static object CopyTo(this object o, object to)
        {
            if (to == null) return null;
            var toType = to.GetType();
            var oType = o.GetType();
            var properties = oType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var property in properties)
            {
                if (!to.HasProperty(property.Name)) continue;
                var value = property.GetValue(o, null);
                to.TrySetValue(property.Name, value);
            }
            return to;
        }
    }

    public class PropertyObject
    {
        public PropertyInfo Property { get; set; }

        public object CurrentObject { get; set; }

        public object Value { get; set; }
    }
}