using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public class DataRowTypeConverter : ITypeConverter
    {
        public object Convert(object o, Type t)
        {
            var value = Activator.CreateInstance(t);
            var row = o as DataRow;
            foreach (DataColumn column in row.Table.Columns)
            {
                object obj = row[column];
                if (obj == null) continue;
                value.TrySetValue(column.ColumnName, obj);
            }
            return value;
        }

        public T Convert<T>(object o)
        {
            return (T)Convert(o, typeof(T));
        }
    }
}
