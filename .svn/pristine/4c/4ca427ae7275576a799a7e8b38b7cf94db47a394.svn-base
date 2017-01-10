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
            StringBuilder sbSQL = new StringBuilder();
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
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.AppendFormat(" SELECT COUNT(*) FROM ({0}) Temp ", SQL);
            return sbSQL.ToString();
        }
    }
}
