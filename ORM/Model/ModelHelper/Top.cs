using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Frank.Common.DAL;

namespace SOAFramework.ORM.ORMDriver
{
    partial class TableModelHelper<T>
    {
        #region Top
        public TableModelHelper<T> Top(int TopN)
        {
            DataRow drNew = _dt_Condition.NewRow();
            drNew["Condition"] = "top";
            drNew["Value"] = TopN.ToString();
            _dt_Condition.Rows.Add(drNew);
            return this;
        }
        #endregion
    }
}
