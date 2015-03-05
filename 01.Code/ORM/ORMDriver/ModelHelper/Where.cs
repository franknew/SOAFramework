using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using SOAFramework.ORM.Common;

namespace SOAFramework.ORM.ORMDriver
{
    partial class TableModelHelper<T>
    {
        public TableModelHelper<T> Where(string Where = null, ORMParameter[] Parameters = null)
        {
            DataRow drNew = _dt_Condition.NewRow();
            drNew["Condition"] = "where";
            drNew["Value"] = Where;
            drNew["Parameters"] = ORMParameter.ToDALParameter(Parameters).ToArray();
            _dt_Condition.Rows.Add(drNew);
            return this;
        }

        //public ModelHelper<T> Where(string Where)
        //{
        //    return this.Where(Where, null);
        //}
    }
}
