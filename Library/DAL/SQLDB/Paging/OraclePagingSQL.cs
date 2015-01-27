using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL
{
    public class OraclePagingSQL : IPagingSQL
    {
        public string GetPagingSQL(string SQL, string OrderBy, int StartIndex, int EndIndex)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT * FROM (SELECT t.*, rownum as _rownum FROM ({0}) t) WHERE _rownum>={1} AND rownum<={2}", SQL, StartIndex, EndIndex);
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
