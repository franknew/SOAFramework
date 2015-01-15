using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SOAFramework.Library.DAL;

namespace SOAFramework.ORM.ORMDriver
{
    partial class TableModelHelper<T> 
    {
        #region GetModelList
        public List<T> GetModelList(int PageIndex = -1, int PageSize = -1, string IDColumnName = null, string DBEx = null)
        {
            List<T> lstReturn = null;
            IDBHelper objHelper = DBFactory.CreateDBHelper();
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
                lstReturn = AssignData(dtModels);
            }
            return lstReturn;
        }

        //public List<T> GetModelList(int PageIndex, int PageSize, string IDColumnName)
        //{
        //    return GetModelList(PageIndex, PageSize, IDColumnName, "");
        //}

        //public List<T> GetModelList()
        //{
        //    return GetModelList(-1, -1, null, "");
        //}

        //public List<T> GetModelList(string DBEx)
        //{
        //    return GetModelList(-1, -1, null, DBEx);
        //}
        #endregion
    }
}
