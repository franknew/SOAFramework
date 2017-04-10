using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SOAFramework.Library.DAL;

namespace SOAFramework.ORM.ORMDriver
{
    partial class TableModelHelper<T>
    {
        #region GetCount
        public int GetCount(string DBEx = null)
        {
            IDBHelper objHelper = DBFactory.CreateDBHelper();
            int intCount = 0;
            Parameter[] objParameters = null;
            string strSQL = GetSQL(DBEx, "getcount", "", out objParameters);
            DataTable dtModel = objHelper.GetTableWithSQL(strSQL, objParameters);
            if (dtModel != null && dtModel.Rows.Count > 0 && dtModel.Rows[0][0] != DBNull.Value)
            {
                intCount = Convert.ToInt32(dtModel.Rows[0][0]);
            }
            return intCount;
        }

        //public int GetCount()
        //{
        //    return GetCount("");
        //}
        #endregion
    }
}
