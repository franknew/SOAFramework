using System;
using System.Collections.Generic;
using System.Text;

namespace SOAFramework.Library.DAL
{
    public class MSSQLPagingSQL : IPagingSQL
    {
        /// <summary>
        /// 根据SQL生成分页的SQL
        /// </summary>
        /// <param name="SQL">SQL</param>
        /// <param name="OrderBy">排序字段</param>
        /// <param name="StartIndex">开始索引</param>
        /// <param name="EndIndex">结束索引</param>
        /// <returns></returns>
        public string GetPagingSQL(string SQL, string OrderByColumn, int StartIndex, int EndIndex, OrderBy orderby = OrderBy.ASC)
        {
            if (StartIndex < 0 || EndIndex < 0) throw new Exception("start index和end index必须大于-1");
            if (string.IsNullOrEmpty(OrderByColumn)) throw new Exception("OrderByColumn不能为空");
            if (string.IsNullOrEmpty(SQL)) throw new Exception("SQL不能为空");
            StringBuilder sbSQL = new StringBuilder();
            var orderIndex = SQL.ToUpper().IndexOf(" ORDER BY ");
            if (orderIndex > -1) SQL = SQL.Remove(orderIndex);
            sbSQL.AppendFormat("SELECT * FROM (SELECT *, ROW_NUMBER() OVER(ORDER BY {0} {2}) AS _RowNumber FROM ({1}) Temp1 ) Temp", OrderByColumn, SQL, orderby.ToString());
            sbSQL.AppendFormat(" WHERE _RowNumber BETWEEN {0} AND {1} ", StartIndex, EndIndex);
            return sbSQL.ToString();
        }
        /// <summary>
        /// 获得数据总数
        /// </summary>
        /// <param name="SQL"></param>
        /// <returns></returns>
        public string GetCountSQL(string SQL)
        {
            if (string.IsNullOrEmpty(SQL)) throw new Exception("SQL不能为空");
            StringBuilder sbSQL = new StringBuilder();
            var sql = SQL.Substring(SQL.ToUpper().IndexOf(" FROM "));
            var orderIndex = sql.ToUpper().IndexOf(" ORDER BY ");
            if (orderIndex > -1) sql = sql.Remove(orderIndex);
            sbSQL.AppendFormat("SELECT COUNT(1) {0}", sql);
            return sbSQL.ToString();
        }

        public string BuildOrderBy(string sql, string orderByColumn, OrderBy orderby = OrderBy.ASC)
        {
            if (string.IsNullOrEmpty(orderByColumn)) throw new Exception("OrderByColumn不能为空");
            if (string.IsNullOrEmpty(sql)) throw new Exception("SQL不能为空");
            if (string.IsNullOrEmpty(orderByColumn)) return sql;
            StringBuilder builder = new StringBuilder();
            var orderIndex = sql.ToUpper().IndexOf(" ORDER BY ");
            if (orderIndex > -1) sql = sql.Remove(orderIndex);
            builder.AppendFormat("{0} ORDER BY {1} {2}", sql, orderByColumn, orderby.ToString());
            return builder.ToString();
        }
    }
}
