using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Frank.Common.DAL;

namespace SOAFramework.ORM.ORMDriver
{
    partial class TableModelHelper<T>
    {
        /// <summary>
        /// 对某个字段汇总
        /// </summary>
        /// <param name="ColumnName">字段名</param>
        /// <param name="DBEx">数据库前缀</param>
        /// <returns></returns>
        public double GetSum(string ColumnName = null, string DBEx = null)
        {
            DBHelper objHelper = DBFactory.CreateDBHelper();
            double dblReturn = 0;
            if (!string.IsNullOrEmpty(ColumnName))
            {
                Parameter[] objParameters = null;
                string strSQL = GetSQL(DBEx, "getsum", ColumnName, out objParameters);
                DataTable dtModel = objHelper.GetTableWithSQL(strSQL, objParameters);
                if (dtModel != null && dtModel.Rows.Count > 0 && dtModel.Rows[0][0] != DBNull.Value)
                {
                    dblReturn = Convert.ToDouble(dtModel.Rows[0][0]);
                }
            }
            return dblReturn;
        }

        //public double GetSum(string ColumnName)
        //{
        //    return GetSum(ColumnName, "");
        //}
    }
}
