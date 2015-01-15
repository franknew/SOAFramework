using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SOAFramework.Library.DAL;

namespace SOAFramework.ORM.ORMDriver
{
    partial class TableModelHelper<T>
    {
        /// <summary>
        /// 获得某个字段的最小值
        /// </summary>
        /// <param name="ColumnName">字段名</param>
        /// <param name="DBEx">数据库前缀</param>
        /// <returns></returns>
        public object GetMin(string ColumnName = null, string DBEx = null)
        {
            IDBHelper objHelper = DBFactory.CreateDBHelper();
            object objReturn = null;
            if (!string.IsNullOrEmpty(ColumnName))
            {
                Parameter[] objParameters = null;
                string strSQL = GetSQL(DBEx, "getmin", ColumnName, out objParameters);
                DataTable dtModel = objHelper.GetTableWithSQL(strSQL);
                if (dtModel != null && dtModel.Rows.Count > 0 && dtModel.Rows[0][0] != DBNull.Value)
                {
                    objReturn = dtModel.Rows[0][0];
                }
            }
            return objReturn;
        }

        //public object GetMin(string ColumnName)
        //{
        //    return GetMin(ColumnName, "");
        //}
    }
}
