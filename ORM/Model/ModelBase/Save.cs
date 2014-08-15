using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Frank.Common.DAL;
using SOAFramework.ORM.Common;

namespace SOAFramework.ORM.ORMDriver
{
    partial class TableModelBase<T> where T : new()
    {
        #region Save
        /// <summary>
        /// 保存数据，自动判断是新增还是更新
        /// </summary>
        /// <returns>主键ID,主键必须要整型</returns>
        public object Save(Transaction Transaction = null)
        {
            StringBuilder sbSQL = new StringBuilder();
            StringBuilder sbSQL_Value = new StringBuilder();
            StringBuilder sbSQL_Columns = new StringBuilder();
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
            object objReturn = null;
            if (_bl_IsNew)
            {
                if (dtProperties != null && dtProperties.Columns.Count > 0 && dtProperties.Rows.Count > 0)
                {
                    DataRow drProperty = dtProperties.Rows[0];
                    sbSQL.Append(" INSERT INTO ");
                    sbSQL.Append(_cad_TableDef.GetFullTableName(DBExString));
                    sbSQL.Append("(");
                    int x = 0;
                    for (int i = 0; i < _dt_ClassDef.Columns.Count; i++)
                    {
                        if ((drProperty[_dt_ClassDef.Columns[i].ColumnName] != DBNull.Value && !Convert.ToBoolean(_dt_ClassDef.Rows[3][i])
                        && null != _dt_ValueCopy
                        && !drProperty[_dt_ClassDef.Columns[i].ColumnName].Equals(_dt_ValueCopy.Rows[0][_dt_ClassDef.Columns[i].ColumnName]))
                        || (drProperty[_dt_ClassDef.Columns[i].ColumnName] != DBNull.Value && !Convert.ToBoolean(_dt_ClassDef.Rows[3][i])
                        && null == _dt_ValueCopy))
                        {
                            if (x == 0)
                            {
                                sbSQL_Columns.Append("[");
                                sbSQL_Columns.Append(_dt_ClassDef.Columns[i].ColumnName);
                                sbSQL_Columns.Append("]");

                                sbSQL_Value.Append("@");
                                sbSQL_Value.Append(_dt_ClassDef.Columns[i].ColumnName);
                                x++;
                            }
                            else
                            {
                                sbSQL_Columns.Append(",[");
                                sbSQL_Columns.Append(_dt_ClassDef.Columns[i].ColumnName);
                                sbSQL_Columns.Append("]");

                                sbSQL_Value.Append(",@");
                                sbSQL_Value.Append(_dt_ClassDef.Columns[i].ColumnName);
                                x++;
                            }
                        }
                    }
                    if (0 == x)//x=0说明没有赋值便进行保存
                    {
                        return -1;
                    }
                    sbSQL.Append(sbSQL_Columns.ToString());
                    sbSQL.Append(") VALUES(");
                    sbSQL.Append(sbSQL_Value.ToString());
                    sbSQL.Append(")");
                }
            }
            else
            {
                if (dtProperties != null && dtProperties.Columns.Count > 0 && dtProperties.Rows.Count > 0)
                {
                    DataRow drProperty = dtProperties.Rows[0];
                    sbSQL.Append(" UPDATE ");
                    sbSQL.Append(_cad_TableDef.GetFullTableName(DBExString));
                    sbSQL.Append(" SET ");
                    int x = 0;
                    for (int i = 0; i < _dt_ClassDef.Columns.Count; i++)
                    {
                        if (x == 0)
                        {
                            if ((_dt_ClassDef.Rows[0][i].ToString() == "0" && drProperty[_dt_ClassDef.Columns[i].ColumnName] != DBNull.Value
                            && null != _dt_ValueCopy
                            && !drProperty[_dt_ClassDef.Columns[i].ColumnName].Equals(_dt_ValueCopy.Rows[0][_dt_ClassDef.Columns[i].ColumnName]))
                            || (_dt_ClassDef.Rows[0][i].ToString() == "0" && drProperty[_dt_ClassDef.Columns[i].ColumnName] != DBNull.Value
                            && null == _dt_ValueCopy))
                            {
                                sbSQL.Append("[");
                                sbSQL.Append(_dt_ClassDef.Columns[i].ColumnName);
                                sbSQL.Append("]");
                                sbSQL.Append("=");
                                sbSQL.Append("@");
                                sbSQL.Append(_dt_ClassDef.Columns[i].ColumnName);
                                x++;
                            }
                        }
                        else
                        {
                            if ((_dt_ClassDef.Rows[0][i].ToString() == "0" && drProperty[_dt_ClassDef.Columns[i].ColumnName] != DBNull.Value
                            && null != _dt_ValueCopy
                            && !drProperty[_dt_ClassDef.Columns[i].ColumnName].Equals(_dt_ValueCopy.Rows[0][_dt_ClassDef.Columns[i].ColumnName]))
                            || (_dt_ClassDef.Rows[0][i].ToString() == "0" && drProperty[_dt_ClassDef.Columns[i].ColumnName] != DBNull.Value
                            && null == _dt_ValueCopy))
                            {
                                sbSQL.Append(", [");
                                sbSQL.Append(_dt_ClassDef.Columns[i].ColumnName);
                                sbSQL.Append("]");
                                sbSQL.Append("=");
                                sbSQL.Append("@");
                                sbSQL.Append(_dt_ClassDef.Columns[i].ColumnName);
                                x++;
                            }
                        }
                    }
                    if (0 == x)//x=0说明没有赋新值便进行保存
                    {
                        return -1;
                    }
                    sbSQL.Append(" WHERE ");
                    for (int i = 0, y = 0; i < _dt_ClassDef.Columns.Count; i++)
                    {
                        if (_dt_ClassDef.Rows[0][i].ToString() == "1")
                        {
                            if (y == 0)
                            {
                                if (drProperty[_dt_ClassDef.Columns[i].ColumnName] != DBNull.Value)
                                {
                                    sbSQL.Append("[");
                                    sbSQL.Append(_dt_ClassDef.Columns[i].ColumnName);
                                    sbSQL.Append("=@");
                                    sbSQL.Append(_dt_ClassDef.Columns[i].ColumnName);
                                    objReturn = drProperty[_dt_ClassDef.Columns[i].ColumnName];
                                    y++;
                                }
                            }
                            else
                            {
                                if (drProperty[_dt_ClassDef.Columns[i].ColumnName] != DBNull.Value)
                                {
                                    sbSQL.Append(" AND ");
                                    sbSQL.Append("[");
                                    sbSQL.Append(_dt_ClassDef.Columns[i].ColumnName);
                                    sbSQL.Append("]=@");
                                    sbSQL.Append(_dt_ClassDef.Columns[i].ColumnName);
                                    y++;
                                }
                            }
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(sbSQL.ToString()))
            {
                Parameter[] objParameters = null;
                if (dtProperties != null && dtProperties.Rows.Count > 0 && dtProperties.Columns.Count > 0)
                {
                    objParameters = new Parameter[dtProperties.Columns.Count];
                    for (int i = 0; i < dtProperties.Columns.Count; i++)
                    {
                        objParameters[i] = new Parameter();
                        objParameters[i].Name = "@" + dtProperties.Columns[i].ColumnName;
                        objParameters[i].Value = dtProperties.Rows[0][i];
                    }
                }
                DataTable dtReturn = objHelper.GetTableWithSQL(sbSQL.ToString(), objParameters);
                if (null != dtReturn && dtReturn.Rows.Count > 0)
                {
                    objReturn = Convert.ToInt32(dtReturn.Rows[0][0]);
                }
            }
            //更新完以后复制一份数据以用来接下来的更新或者保存方面的比较
            this.CopyValue();
            return objReturn;
        }

        //public object Save()
        //{
        //    return Save(null);
        //}
        #endregion
    }
}