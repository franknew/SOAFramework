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
        public static DataTable ToDataTable(this object[] list, List<ToDataTableMapping> mappinglist = null)
        {
            return ArrayToDataTable(list, mappinglist);
        }

        /// <summary>
        /// 转换成DataTable,如果没有数据能自动生成架构
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IList<T> list, List<ToDataTableMapping> mappinglist = null)
        {
            DataTable table = new DataTable();
            //如果没有数据则生成架构
            if (list == null || list.Count == 0)
            {
                table = typeof(T).GetSchema(mappinglist);
            }
            else
            {
                object[] array = new object[list.Count];
                for (int i = 0; i < list.Count; i++)
                {
                    array[i] = list[i];
                }
                table = ArrayToDataTable(list, mappinglist);
            }
            table.AcceptChanges();
            return table;
        }

        private static DataTable ArrayToDataTable(IEnumerable list, List<ToDataTableMapping> mappinglist = null)
        {
            DataTable table = new DataTable();
            var enumerator = list.GetEnumerator();
            if (enumerator.MoveNext())
            {
                //create data table schema
                object schema = enumerator.Current;
                if (schema is IDictionary)
                {
                    IDictionary<string, object> dic = schema as Dictionary<string, object>;
                    foreach (var data in list)
                    {
                        IDictionary<string, object> dicData = data as Dictionary<string, object>;
                        DataRow row = table.NewRow();
                        foreach (DataColumn col in table.Columns)
                        {
                            row[col] = dicData[col.ColumnName];
                        }
                        table.Rows.Add(row);
                    }
                }
                else
                {
                    var objType = schema.GetType();
                    table = objType.GetSchema(mappinglist);
                    foreach (var data in list)
                    {
                        DataRow row = table.NewRow();
                        foreach (DataColumn col in table.Columns)
                        {
                            if (col.ColumnName.Contains("Entity")) continue;
                            var property = objType.GetProperty(col.ColumnName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
                            Type t = Nullable.GetUnderlyingType(property.PropertyType)
                                ?? property.PropertyType;
                            object value = property.GetValue(data, null);
                            object safeValue = (value == null || value == DBNull.Value) ? DBNull.Value
                                                               : Convert.ChangeType(value, t);
                            row[col] = safeValue;
                        }
                        table.Rows.Add(row);
                    }
                }
            }
            table.AcceptChanges();
            if (mappinglist != null)
            {
                foreach (DataColumn col in table.Columns)
                {
                    col.ColumnName = mappinglist.Find(t => t.OrignalName.Equals(col.ColumnName))?.MappingName;
                }
            }
            return table;
        }

        public static DataTable GetSchema(this Type type, List<ToDataTableMapping> mappinglist = null)
        {
            DataTable table = new DataTable();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string columnName = property.Name;
                Type t = Nullable.GetUnderlyingType(property.PropertyType)
                    ?? property.PropertyType;
                if (mappinglist != null)
                {
                    var mapping = mappinglist.Find(p => p.OrignalName.Equals(columnName));
                    if (mapping != null)
                    {
                        DataColumn column = new DataColumn(columnName, t);
                        if (!table.Columns.Contains(column.ColumnName)) table.Columns.Add(column);
                    }
                }
                else
                {
                    DataColumn column = new DataColumn(columnName, t);
                    if (!table.Columns.Contains(column.ColumnName)) table.Columns.Add(column);
                }
            }
            return table;
        }
        public static IDictionary<TKey, TResult> MapReduce<TInput, TKey, TValue, TResult>(this List<TInput> inputList,
            Func<MapReduceData<TInput>, KeyValueClass<TKey, TValue>> map, Func<TKey, IList<TValue>, TResult> reduce)
        {
            return inputList.MapReduce<TInput, TKey, TValue, TResult>(map, reduce);
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

    public class ToDataTableMapping
    {
        public string OrignalName { get; set; }
        public string MappingName { get; set; }
    }
}
