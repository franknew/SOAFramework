using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Frank.Common.DAL;

namespace SOAFramework.ORM.ORMDriver
{
    partial class TableModelHelper<T>
    {
        #region GetDistinct
        public TableModelHelper<T> Distinct(string ColumnName)
        {
            DataRow drNew = _dt_Condition.NewRow();
            drNew["Condition"] = "distinct";
            drNew["Value"] = ColumnName;
            _dt_ClassDef.Rows.Add(drNew);
            return this;
        }
        #endregion
    }
}
