using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Frank.Common.DAL;

using SOAFramework.ORM.Common;

namespace SOAFramework.ORM.ORMDriver
{
    partial class TableModelBase<T> where T : new()
    {
        public bool Delete(Transaction Transaction = null)
        {
            bool blResult = false;
            DBHelper objHelper = null;
            DataTable dtPropertiesAndValue = Reflection.GetClassValues(this, _dt_ClassDef);
            List<Parameter> lstDALParams = new List<Parameter>();
            if (null != Transaction && null != Transaction.DBHelper)
            {
                objHelper = Transaction.DBHelper;
            }
            else
            {
                objHelper = DBFactory.CreateDBHelper();
            }
            if (dtPropertiesAndValue != null && dtPropertiesAndValue.Rows.Count > 0)
            {
                StringBuilder sbSQL = new StringBuilder();
                sbSQL.Append(" DELETE FROM ");
                sbSQL.Append(_cad_TableDef.GetFullTableName(DBExString));
                sbSQL.Append(" WHERE ");
                int i = 0;
                foreach (DataColumn dcTemp in _dt_ClassDef.Columns)
                {
                    if (i == 0)
                    {
                        if (_dt_ClassDef.Rows[0][dcTemp].ToString() == "1")
                        {
                            sbSQL.Append(dcTemp.ColumnName).Append("=@").Append(dcTemp.ColumnName);
                            Parameter objParam = new Parameter(dcTemp.ColumnName, dtPropertiesAndValue.Rows[0][dcTemp]);
                            lstDALParams.Add(objParam);
                            i++;
                        }
                    }
                    else
                    {
                        if (_dt_ClassDef.Rows[0][dcTemp].ToString() == "1")
                        {
                            sbSQL.Append(" AND").Append(dcTemp.ColumnName).Append("=@").Append(dcTemp.ColumnName);
                            Parameter objParam = new Parameter(dcTemp.ColumnName, dtPropertiesAndValue.Rows[0][dcTemp]);
                            lstDALParams.Add(objParam);
                            i++;
                        }
                    }
                }
                objHelper.ExecNoneQueryWithSQL(sbSQL.ToString(), lstDALParams.ToArray());
            }
            return blResult;
        }
        //public bool Delete()
        //{
        //    return Delete(null);
        //}
    }
}
