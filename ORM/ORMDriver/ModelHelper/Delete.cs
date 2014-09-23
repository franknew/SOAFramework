using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Frank.Common.DAL;

namespace SOAFramework.ORM.ORMDriver
{
    partial class TableModelBase<T> where T : new()
    {
        //public static int Delete(Transaction Transaction)
        //{
        //    int intCount = 0;
        //    DataTable dtPropertiesAndValue = Reflection.GetClassValues(this, _dt_ClassDef);
        //    DBHelper objHelper = null;
        //    if (null != Transaction && null != Transaction.DBHelper)
        //    {
        //        objHelper = Transaction.DBHelper;
        //    }
        //    else
        //    {
        //        objHelper = DBFactory.CreateDBHelper();
        //    }
        //    if (dtPropertiesAndValue != null && dtPropertiesAndValue.Rows.Count > 0)
        //    {
        //        StringBuilder sbSQL = new StringBuilder();
        //        sbSQL.Append(" DELETE FROM ");
        //        sbSQL.Append(_cad_TableDef.GetFullTableName(DBExString));
        //        sbSQL.Append(" WHERE ");
        //        int i = 0;
        //        foreach (DataColumn dcTemp in _dt_ClassDef.Columns)
        //        {
        //            if (i == 0)
        //            {
        //                if (_dt_ClassDef.Rows[0][dcTemp].ToString() == "1")
        //                {
        //                    sbSQL.Append(dcTemp.ColumnName + "='" + dtPropertiesAndValue.Rows[0][dcTemp].ToString() + "' ");
        //                    i++;
        //                }
        //            }
        //            else
        //            {
        //                if (_dt_ClassDef.Rows[0][dcTemp].ToString() == "1")
        //                {
        //                    sbSQL.Append(" AND" + dcTemp.ColumnName + "='" + dtPropertiesAndValue.Rows[0][dcTemp].ToString() + "' ");
        //                    i++;
        //                }
        //            }
        //        }
        //        intCount = objHelper.ExecNoneQueryWithSQL(sbSQL.ToString());
        //    }
        //    return intCount;
        //}

        //protected static int Delete3Param(string Value1, string Value2, string Value3, string DBEx)
        //{
        //    int intCount = 0;
        //    StringBuilder sbSQL = new StringBuilder();
        //    DBHelper objHelper = null;
        //    if (null != Transaction && null != Transaction.DBHelper)
        //    {
        //        objHelper = Transaction.DBHelper;
        //    }
        //    else
        //    {
        //        objHelper = DBFactory.CreateDBHelper();
        //    }
        //    DataTable dtClassDef = Reflection.GetClassDef(new T());
        //    string strTableName = dtClassDef.TableName;
        //    sbSQL.Append("DELETE FROM ");
        //    sbSQL.Append(_cad_TableDef.GetFullTableName(DBExString));
        //    sbSQL.Append(" WHERE ");
        //    int i = 0;
        //    foreach (DataColumn dcTemp in dtClassDef.Columns)
        //    {
        //        if (!string.IsNullOrEmpty(Value1) && dtClassDef.Rows[0][dcTemp].ToString() == "1" && i == 0)
        //        {
        //            sbSQL.Append(dcTemp.ColumnName + "='" + Value1 + "' ");
        //            i++;
        //        }
        //        else if (string.IsNullOrEmpty(Value1))
        //        {
        //            break;
        //        }
        //        if (!string.IsNullOrEmpty(Value2) && dtClassDef.Rows[0][dcTemp].ToString() == "1" && i == 1)
        //        {
        //            sbSQL.Append(" AND " + dcTemp.ColumnName + "='" + Value2 + "' ");
        //            i++;
        //        }
        //        else if (string.IsNullOrEmpty(Value2))
        //        {
        //            break;
        //        }
        //        if (!string.IsNullOrEmpty(Value3) && dtClassDef.Rows[0][dcTemp].ToString() == "1" && i == 2)
        //        {
        //            sbSQL.Append(" AND " + dcTemp.ColumnName + "='" + Value3 + "' ");
        //            i++;
        //        }
        //        else if (string.IsNullOrEmpty(Value3))
        //        {
        //            break;
        //        }
        //    }
        //    intCount = objHelper.ExecNoneQueryWithSQL(sbSQL.ToString());
        //    return intCount;
        //}

        //protected static int Delete3Param(string Value1, string Value2, string Value3)
        //{
        //    return Delete3Param(Value1, Value2, Value3, "");
        //}

        //protected static int Delete2Param(string Value1, string Value2, string DBEx)
        //{
        //    return Delete3Param(Value1, Value2, "", DBEx);
        //}

        //protected static int Delete2Param(string Value1, string Value2)
        //{
        //    return Delete3Param(Value1, Value2, "", "");
        //}

        //protected static int Delete1Param(string Value1, string DBEx)
        //{
        //    return Delete3Param(Value1, "", "", DBEx);
        //}

        //protected static int Delete1Param(string Value1)
        //{
        //    return Delete3Param(Value1, "", "", "");
        //}
    }
}
