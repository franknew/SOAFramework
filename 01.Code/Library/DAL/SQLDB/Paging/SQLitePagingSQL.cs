using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL
{
    public class SQLitePagingSQL : IPagingSQL
    {
        public string GetPagingSQL(string SQL, string OrderByColumn, int StartIndex, int EndIndex, OrderBy orderby)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT t.* FROM ({0}) t ORDER BY {1} {4} LIMIT {2} OFFSET {3}", SQL, OrderByColumn, EndIndex - StartIndex, StartIndex, orderby);
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
