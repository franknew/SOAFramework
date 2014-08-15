using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SOAFramework.ORM.Common;

namespace SOAFramework.ORM.ORMDriver
{
    partial class TableModelHelper<T>
    {
        public TableModelHelper<T> OrderBy(string ColumnName, string Order)
        {
            DataRow drNew = _dt_Condition.NewRow();
            drNew["Condition"] = "orderby";
            drNew["Value"] = ColumnName + " " + Order;
            return this;
        }

        public TableModelHelper<T> OrderBy(string ColumnName, Enum_Order Order)
        {
            return OrderBy(ColumnName, Order.ToString());
        }
    }
}
