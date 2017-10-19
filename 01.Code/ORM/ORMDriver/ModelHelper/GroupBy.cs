using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace SOAFramework.ORM.ORMDriver
{
    partial class TableModelHelper<T>
    {
        public TableModelHelper<T> GroupBy(string ColumnName)
        {
            DataRow drNew = _dt_Condition.NewRow();
            drNew["Condition"] = "groupby";
            drNew["Value"] = ColumnName;
            _dt_Condition.Rows.Add(drNew);
            return this;
        }
    }
}
