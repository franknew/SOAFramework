using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL
{
    public class SQLitePagingSQL : IPagingSQL
    {
        public string GetPagingSQL(string SQL, string OrderBy, int StartIndex, int EndIndex)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT t.* FROM ({0}) t ORDER BY {1} LIMIT {2} OFFSET {3}", SQL, OrderBy, EndIndex - StartIndex, StartIndex);
            return builder.ToString();
        }

        public string GetCountSQL(string sql)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT COUNT(*) FROM ({0}) t ");
            return builder.ToString();
        }
    }
}
