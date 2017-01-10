using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using SOAFramework.ORM.Common;

namespace SOAFramework.ORM.ORMDriver
{
    partial class TableModelHelper<T> 
    {
        #region GetModel
        public T GetModel(ORMParameter[] Parameters = null, string DBEx=null)
        {
            T objReturn = default(T);
            List<T> lstModels = this.Top(1).GetModelList(DBEx: DBEx);
            if (null != lstModels && 0 < lstModels.Count)
            {
                objReturn = lstModels[0];
            }
            return objReturn;
        }

        //public T GetModel(ORMParameter[] Parameters)
        //{
        //    return GetModel(Parameters, null);
        //}

        //public T GetModel()
        //{
        //    return GetModel(null, null);
        //}
        #endregion
    }
}
