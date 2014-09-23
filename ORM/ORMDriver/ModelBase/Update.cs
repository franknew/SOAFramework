using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Frank.Common.DAL;
using SOAFramework.ORM.Common;

namespace SOAFramework.ORM.ORMDriver
{
    public partial class TableModelBase<T> where T : new()
    {
        #region Update
        public bool Update(string Where = null, ORMParameter[] Parameters = null, Transaction Transaction = null)
        {
            bool blResult = true;
            StringBuilder sbSQL = new StringBuilder();
            DBHelper objHelper = null;
            if (null != Transaction && null != Transaction.DBHelper)
            {
                objHelper = Transaction.DBHelper;
            }
            else
            {
                objHelper = DBFactory.CreateDBHelper();
            }
            DataTable dtProperties = Reflection.GetClassValues(this, _dt_ClassDef);
            if (dtProperties != null && dtProperties.Columns.Count > 0 && dtProperties.Rows.Count > 0)
            {
                DataRow drProperty = dtProperties.Rows[0];
                sbSQL.Append(" UPDATE ");
                sbSQL.Append(_cad_TableDef.GetFullTableName(DBExString));
                sbSQL.Append(" SET ");
                for (int i = 0, x = 0; i < _dt_ClassDef.Columns.Count; i++)
                {
                    if (x == 0)
                    {
                        if (drProperty[_dt_ClassDef.Columns[i].ColumnName] != DBNull.Value && _dt_ClassDef.Rows[0][i].ToString() == "0")
                        {
                            sbSQL.Append(_dt_ClassDef.Columns[i].ColumnName + "=@" + _dt_ClassDef.Columns[i].ColumnName);
                            x++;
                        }
                    }
                    else
                    {
                        if (drProperty[_dt_ClassDef.Columns[i].ColumnName] != DBNull.Value && _dt_ClassDef.Rows[0][i].ToString() == "0")
                        {
                            sbSQL.Append("," + _dt_ClassDef.Columns[i].ColumnName + "=@" + _dt_ClassDef.Columns[i].ColumnName);
                            x++;
                        }
                    }
                }
                sbSQL.Append(" WHERE ");
                if (!string.IsNullOrEmpty(Where))
                {
                    sbSQL.Append((" " + Where.ToLower()).Replace(" where ", ""));//去掉里面的where
                }
                else
                {
                    for (int i = 0, x = 0; i < _dt_ClassDef.Columns.Count; i++)
                    {
                        if (_dt_ClassDef.Rows[0][i].ToString() == "1")
                        {
                            if (x == 0)
                            {
                                if (drProperty[_dt_ClassDef.Columns[i].ColumnName] != DBNull.Value)
                                {
                                    sbSQL.Append(_dt_ClassDef.Columns[i].ColumnName + "=@" + _dt_ClassDef.Columns[i].ColumnName + "");
                                    x++;
                                }
                            }
                            else
                            {
                                if (drProperty[_dt_ClassDef.Columns[i].ColumnName] != DBNull.Value)
                                {
                                    sbSQL.Append(" AND " + _dt_ClassDef.Columns[i].ColumnName + "=@" + _dt_ClassDef.Columns[i].ColumnName + "");
                                    x++;
                                }
                            }
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(sbSQL.ToString()))
            {
                List<Parameter> lstParameter = new List<Parameter>();
                if (dtProperties != null && dtProperties.Rows.Count > 0 && dtProperties.Columns.Count > 0)
                {
                    for (int i = 0; i < dtProperties.Columns.Count; i++)
                    {
                        Parameter objParameter = new Parameter();
                        objParameter.Name = "@" + dtProperties.Columns[i].ColumnName;
                        objParameter.Value = dtProperties.Rows[0][i];
                        lstParameter.Add(objParameter);
                    }
                }
                lstParameter.AddRange(ORMParameter.ToDALParameter(Parameters));
                objHelper.ExecNoneQueryWithSQL(sbSQL.ToString(), lstParameter.ToArray());
            }
            return blResult;
        }

        //public object Update(string Where)
        //{
        //    return Update(Where, null, null);
        //}

        //public object Update()
        //{
        //    return Update(null, null, null);
        //}

        //public object Update(Transaction Transaction)
        //{
        //    return Update(null, null, Transaction);
        //}

        //public object Update(string Where, Transaction Transaction)
        //{
        //    return Update(Where, null, Transaction);
        //}

        //public object Update(ORMParameter[] Parameters, Transaction Transaction)
        //{
        //    return Update(null, Parameters, Transaction);
        //}
        #endregion
    }
}
