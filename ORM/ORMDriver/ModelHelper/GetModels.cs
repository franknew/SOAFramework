using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Frank.Common.DAL;

namespace SOAFramework.ORM.ModelDriver
{
    partial class ModelHelper<T> 
    {
        #region GetModels
        public T[] GetModels(int PageIndex, int PageSize, string IDColumnName, string DBEx)
        {
            DBHelper objHelper = DBFactory.CreateDBHelper();
            T[] objReturn = null;
            Parameter[] objParameters = null;
            DataTable dtModels = null;
            int intStartIndex = -1;
            int intEndIndex = -1;
            if (PageIndex > 0 && PageSize > 0)
            {
                intStartIndex = (PageIndex - 1) * PageSize;
                intEndIndex = PageIndex * PageSize;
            }
            string strSQL = GetSQL(DBEx, "", "", out objParameters);
            dtModels = objHelper.GetTableWithSQL(strSQL, objParameters, intStartIndex, intEndIndex, IDColumnName);
            if (dtModels != null && dtModels.Rows.Count > 0)
            {
                objReturn = new T[dtModels.Rows.Count];
                objReturn = AssignData(dtModels);
            }
            return objReturn;
        }

        public T[] GetModels(int PageIndex, int PageSize, string IDColumnName)
        {
            return GetModels(PageIndex, PageSize, IDColumnName, "");
        }

        public T[] GetModels()
        {
            return GetModels(-1, -1, null, "");
        }

        public T[] GetModels(string DBEx)
        {
            return GetModels(-1, -1, null, DBEx);
        }
        #endregion
    }
}
