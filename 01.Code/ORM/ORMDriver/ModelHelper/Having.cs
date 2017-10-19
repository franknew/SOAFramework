using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

using SOAFramework.ORM.Common;

namespace SOAFramework.ORM.ORMDriver
{
    partial class TableModelHelper<T>
    {
        /// <summary>
        /// 往条件表中塞入having条件语句
        /// </summary>
        /// <param name="Where"></param>
        /// <returns></returns>
        public TableModelHelper<T> Having(string Where, ORMParameter[] Parameters)
        {
            DataRow drNew = _dt_Condition.NewRow();
            drNew["Condition"] = "having";
            drNew["Value"] = Where;
            drNew["Parameters"] = ORMParameter.ToDALParameter(Parameters).ToArray();
            _dt_Condition.Rows.Add(drNew);
            return this;
        }

        public TableModelHelper<T> Having(string Where)
        {
            return this.Having(Where, null);
        }
    }
}
