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
using System.Linq.Expressions;

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

        public static IDictionary<TKey, TResult> MapReduce<TInput, TKey, TValue, TResult>(this IList<TInput> inputList,
            Func<MapReduceData<TInput>, KeyValueClass<TKey, TValue>> map, Func<TKey, IList<TValue>, TResult> reduce)
        {
            object locker = new object();
            ConcurrentDictionary<TKey, TResult> result = new ConcurrentDictionary<TKey, TResult>();
            //保存map出来的结果
            ConcurrentDictionary<TKey, IList<TValue>> mapDic = new ConcurrentDictionary<TKey, IList<TValue>>();
            var parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = Environment.ProcessorCount;
            //并行map
            Parallel.For(0, inputList.Count(), parallelOptions, t =>
            {
                MapReduceData<TInput> data = new MapReduceData<TInput>
                {
                    Data = inputList[t],
                    Index = t,
                    List = inputList,
                };
                var pair = map(data);
                if (pair != null && pair.Valid)
                {
                    //锁住防止并发操作list造成数据缺失
                    lock (locker)
                    {
                        //将匹配出来的结果加入结果集放入字典
                        IList<TValue> list = null;
                        if (mapDic.ContainsKey(pair.Key))
                        {
                            list = mapDic[pair.Key];
                        }
                        else
                        {
                            list = new List<TValue>();
                            mapDic[pair.Key] = list;
                        }
                        list.Add(pair.Value);
                    }
                }
            });

            //并行reduce
            Parallel.For(0, mapDic.Keys.Count, parallelOptions, t =>
            {
                KeyValuePair<TKey, IList<TValue>> pair = mapDic.ElementAt(t);
                result[pair.Key] = reduce(pair.Key, pair.Value);
            });
            return result;
        }
        public static IDictionary<TKey, TResult> MapReduce<TKey, TValue, TResult>(this DataTable table,
                Func<MapReduceData, KeyValueClass<TKey, TValue>> map, Func<TKey, IList<TValue>, TResult> reduce)
        {
            object locker = new object();
            ConcurrentDictionary<TKey, TResult> result = new ConcurrentDictionary<TKey, TResult>();
            //保存map出来的结果
            ConcurrentDictionary<TKey, IList<TValue>> mapDic = new ConcurrentDictionary<TKey, IList<TValue>>();
            var parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = Environment.ProcessorCount;
            //并行map
            Parallel.For(0, table.Rows.Count, parallelOptions, t =>
            {
                MapReduceData data = new MapReduceData
                {
                    Row = table.Rows[t],
                    Index = t,
                    Table = table,
                };
                var pair = map(data);
                if (pair != null && pair.Valid)
                {
                    //锁住防止并发操作list造成数据缺失
                    lock (locker)
                    {
                        //将匹配出来的结果加入结果集放入字典
                        IList<TValue> list = null;
                        if (mapDic.ContainsKey(pair.Key))
                        {
                            list = mapDic[pair.Key];
                        }
                        else
                        {
                            list = new List<TValue>();
                            mapDic[pair.Key] = list;
                        }
                        list.Add(pair.Value);
                    }
                }
            });

            //并行reduce
            Parallel.For(0, mapDic.Keys.Count, parallelOptions, t =>
            {
                KeyValuePair<TKey, IList<TValue>> pair = mapDic.ElementAt(t);
                result[pair.Key] = reduce(pair.Key, pair.Value);
            });
            return result;
        }

        public static void Set<T, TValue>(this IEnumerable<T> list, Expression<Func<T, TValue>> expression, TValue value)
        {
            foreach (var t in list)
            {
            }
        }
    }

    public class KeyValueClass<K, V>
    {
        public KeyValueClass(K key, V value)
        {
            Key = key;
            Value = value;
            Valid = true;
        }

        public KeyValueClass()
        {

        }
        public KeyValueClass(bool valid)
        {
            Valid = valid;
        }

        public K Key { get; set; }

        public V Value { get; set; }

        public bool Valid { get; set; }

        public static KeyValueClass<K, V> Empty()
        {
            return new KeyValueClass<K, V>(false);
        }
    }

    public class MapReduceData<TInput>
    {
        public TInput Data { get; set; }

        public int Index { get; set; }

        public IList<TInput> List { get; set; }
    }

    public class MapReduceData
    {
        public DataTable Table { get; set; }

        public int Index { get; set; }

        public DataRow Row { get; set; }
    }
}
