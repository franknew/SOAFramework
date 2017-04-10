using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SOAFramework.ORM.Common;

namespace SOAFramework.ORM.ORMDriver
{
    partial class TableModelBase<T> where T : new()
    {
        #region ToDataTable
        public static DataTable ToDataTable(T Model = default(T))
        {
            DataTable dtClassDef = Reflection.GetClassDef(Model);
            return Reflection.GetClassValues(Model, dtClassDef);
        }

        public static DataTable ToDataTable(T[] Models = null)
        {
            DataTable dtReturn = null;
            if (Models != null && Models.Length > 0)
            {
                foreach (T Model in Models)
                {
                    DataTable dtTemp = ToDataTable(Model);
                    if (dtReturn == null)
                    {
                        dtReturn = dtTemp.Copy();
                    }
                    else
                    {
                        if (dtTemp != null && dtTemp.Rows.Count > 0)
                        {
                            object[] objRows = dtTemp.Rows[0].ItemArray;
                            dtReturn.Rows.Add(objRows);
                        }
                    }
                }
            }
            return dtReturn;
        }

        //public DataTable ToDataTable()
        //{
        //    return Reflection.GetClassValues(this, _dt_ClassDef);
        //}
        #endregion
    }
}
