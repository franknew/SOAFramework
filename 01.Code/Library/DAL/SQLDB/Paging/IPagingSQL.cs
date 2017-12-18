using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL
{
    public interface IPagingSQL
    {
        string GetPagingSQL(string SQL, string OrderBy, int StartIndex, int EndIndex, OrderBy orderby = OrderBy.ASC);

        string GetCountSQL(string sql);

        string BuildOrderBy(string sql, string orderByColumn, OrderBy orderby = OrderBy.ASC);
    }

    public class PagingSQLFactory
    {
        public static IPagingSQL Create(string dbType)
        {
            IPagingSQL paging = null;
            switch (dbType.ToLower())
            {
                case "sqlserver2005":
                    paging = new MSSQLPagingSQL();
                    break;
                case "mysql":
                    paging = new MySQLPagingSQL();
                    break;
            }
            return paging;
        }
    }
}
