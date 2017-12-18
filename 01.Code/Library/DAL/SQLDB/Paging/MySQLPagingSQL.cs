using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL
{
    public class MySQLPagingSQL : IPagingSQL
    {
        public string GetPagingSQL(string SQL, string OrderBy, int StartIndex, int EndIndex, OrderBy orderby = OrderBy.ASC)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("{0} LIMIT {1}, {2}", sql, StartIndex, EndIndex - StartIndex);
            return sql.ToString();
        }

        public string GetCountSQL(string sql)
        {
            StringBuilder sqlbuilder = new StringBuilder();
            sqlbuilder.AppendFormat(" SELECT COUNT(*) FROM ({0}) Temp ", sql);
            return sqlbuilder.ToString();
        }

        public string BuildOrderBy(string sql, string orderByColumn, OrderBy orderby = OrderBy.ASC)
        {
            throw new NotImplementedException();
        }
    }
}
