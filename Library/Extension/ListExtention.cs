using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace SOAFramework.Library
{
    public static class ListExtention
    {
        public static List<T> Copy<T>(this IList<T> list)
        {
            T[] array = new T[list.Count];
            List<T> copyList = new List<T>();
            list.CopyTo(array, 0);
            copyList.AddRange(array);
            return copyList;
        }
        
      
        /// <summary>
        /// 转换成DataTable
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(this Object[] list)
        {
            DataTable table = new DataTable();
            if (list != null && list.Length > 0)
            {
                //create data table schema
                object schema = list[0];
                if (schema is IDictionary)
                {
                    IDictionary<string, object> dic = schema as Dictionary<string, object>;
                    foreach (var data in list)
                    {
                        IDictionary<string, object> dicData = data as Dictionary<string, object>;
                        DataRow row = table.NewRow();
                        foreach (var key in dicData.Keys)
                        {
                            if (!table.Columns.Contains(key))
                            {
                                table.Columns.Add(key, dicData[key].GetType());
                            }
                            row[key] = dicData[key];
                        }
                        table.Rows.Add(row);
                    }
                }
                else
                {
                    PropertyInfo[] properties = schema.GetType().GetProperties();
                    foreach (PropertyInfo property in properties)
                    {
                        if (property.Name.Contains("Entity"))
                        {
                            continue;
                        }
                        Type t = Nullable.GetUnderlyingType(property.PropertyType)
                            ?? property.PropertyType;
                        table.Columns.Add(property.Name, t);
                    }
                    foreach (var data in list)
                    {
                        DataRow row = table.NewRow();
                        foreach (PropertyInfo property in properties)
                        {
                            if (property.Name.Contains("Entity"))
                            {
                                continue;
                            }
                            Type t = Nullable.GetUnderlyingType(property.PropertyType)
                                ?? property.PropertyType;
                            object value = property.GetValue(data, null);
                            object safeValue = (value == null || value == DBNull.Value) ? DBNull.Value
                                                               : Convert.ChangeType(value, t);
                            row[property.Name] = safeValue;
                        }
                        table.Rows.Add(row);
                    }
                }
            }
            table.AcceptChanges();
            return table;
        }

        /// <summary>
        /// 转换成DataTable
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        //public static DataTable ToDataTable(this IList list)
        //{

        //    if (list == null) return null;

        //    object[] array = new object[list.Count];
        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        array[i] = list[i];
        //    }
        //     return array.ToDataTable();
        //}

        /// <summary>
        /// 转换成DataTable,如果没有数据能自动生成架构
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IList<T> list)
        {
            DataTable table = new DataTable();
            //如果没有数据则生成架构
            if (list == null || list.Count == 0)
            {
                PropertyInfo[] properties = typeof(T).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    Type t = Nullable.GetUnderlyingType(property.PropertyType)
                        ?? property.PropertyType;
                    DataColumn column = new DataColumn(property.Name, t);
                    if (!table.Columns.Contains(column.ColumnName))
                    {
                        table.Columns.Add(column);
                    }
                }
            }
            else
            {
                object[] array = new object[list.Count];
                for (int i = 0; i < list.Count; i++)
                {
                    array[i] = list[i];
                }
                table = array.ToDataTable();
            }
            table.AcceptChanges();
            return table;
        }
    }
}
