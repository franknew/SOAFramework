using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Frank.Common.DAL;
using SOAFramework.ORM.Common;

namespace SOAFramework.ORM.ORMDriver
{
    partial class TableModelHelper<T>
    {
        #region IsExits
        public bool IsExists(string Where = null, ORMParameter[] Parameters = null, string DBEx = null)
        {
            DBHelper objHelper = DBFactory.CreateDBHelper();
            StringBuilder sbSQL = new StringBuilder();
            List<Parameter> lstDALParameters = new List<Parameter>();
            bool blHasValue = false;
            string strTableName = _dt_ClassDef.TableName;
            sbSQL.Append(" SELECT TOP 1 ");
            if (_dt_ClassDef != null)
            {
                sbSQL.Append(_dt_ClassDef.Columns[0].ColumnName);
            }
            else
            {
                sbSQL.Append(" * ");
            }
            sbSQL.Append(" FROM ");
             sbSQL.Append(_cad_TableDef.GetFullTableName(DBEx));
            sbSQL.Append(" WHERE ");
            if (!string.IsNullOrEmpty(Where))
            {
                sbSQL.Append("(" + Where + ")");
            }
            if (Parameters != null && string.IsNullOrEmpty(Where))
            {
                foreach (ORMParameter objParameter in Parameters)
                {
                    Parameter objDALParam = new Parameter(objParameter.Name, objParameter.Value);
                    lstDALParameters.Add(objDALParam);
                }
            }
            DataTable dtModel = objHelper.GetTableWithSQL(sbSQL.ToString(), lstDALParameters.ToArray());
            if (dtModel != null && dtModel.Rows.Count > 0)
            {
                blHasValue = true;
            }
            return blHasValue;
        }

        //public bool IsExists(string Where, string DBEx)
        //{
        //    return IsExists(Where, null, DBEx);
        //}


        //public bool IsExists(string Where, ORMParameter[] Parameters)
        //{
        //    return IsExists(Where, Parameters, null);
        //}

        //public bool IsExists(string Where)
        //{
        //    return IsExists(Where, null, null);
        //}

        //public bool IsExists(ORMParameter[] Parameters, string DBEx)
        //{
        //    return IsExists(null, Parameters, DBEx);
        //}

        //public bool IsExists(ORMParameter[] Parameters)
        //{
        //    return IsExists(null, Parameters, null);
        //}
        #endregion
    }
}
