using System;
using System.Collections.Generic;
using System.Text;

namespace Frank.Common.DAL
{
    public class PagingSQL
    {
        /// <summary>
        /// 根据SQL生成分页的SQL
        /// </summary>
        /// <param name="SQL">SQL</param>
        /// <param name="OrderBy">排序字段</param>
        /// <param name="StartIndex">开始索引</param>
        /// <param name="EndIndex">结束索引</param>
        /// <returns></returns>
        public static string GetPagingSQL(string SQL, string OrderBy, int StartIndex, int EndIndex)
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append("SELECT * FROM (SELECT *, ROW_NUMBER() OVER(ORDER BY " + OrderBy + ") AS RowNumber FROM (" + SQL + ") Temp1 ) Temp");
            sbSQL.Append(" WHERE RowNumber BETWEEN " + StartIndex + " AND " + EndIndex + " ");
            return sbSQL.ToString();
        }
        /// <summary>
        /// 获得数据总数
        /// </summary>
        /// <param name="SQL"></param>
        /// <returns></returns>
        public static string GetCountSQL(string SQL)
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append(" SELECT COUNT(*) FROM (" + SQL + ") Temp ");
            return sbSQL.ToString();
        }
    }
}
