using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace Frank.Common.DAL
{
    public class Parameter
    {
        #region variables
        private object mStr_Value;
        private string mStr_Name;
        private Type obj_Type;
        #endregion

        #region constructor
        public Parameter(string strParameterName, object objValue)
        {
            mStr_Name = strParameterName;
            obj_Type = objValue.GetType();
            mStr_Value = objValue;
        }

        public Parameter(string strParameterName, object objValue, int intLength)
        {
            mStr_Name = strParameterName;;
            obj_Type = objValue.GetType();
            mStr_Value = objValue;
            if (objValue.ToString().Length >= intLength)
            {
                mStr_Value = objValue.ToString().Substring(0, intLength);
            }
        }

        public Parameter()
        {
        }
        #endregion

        #region attributes
        public object Value
        {
            get { return mStr_Value; }
            set 
            { 
                obj_Type = value.GetType();
                mStr_Value = value;
            }
        }

        public string Name
        {
            get { return mStr_Name; }
            set { mStr_Name = value; }
        }
        public Type Type
        {
            get { return obj_Type; }
            set { obj_Type = value; }
        }
        #endregion

        #region method
        public static SqlParameter[] ChangeToSqlParameters(Parameter[] objParams)
        {
            SqlParameter[] objSqlParams = null;
            if (objParams != null)
            {
                objSqlParams = new SqlParameter[objParams.Length];
                for (int i = 0; i < objParams.Length; i++)
                {
                    objSqlParams[i] = new SqlParameter(objParams[i].Name, Convert.ChangeType(objParams[i].Value, objParams[i].Type));
                }
            }
            return objSqlParams;
        }
        public static OleDbParameter[] ChangeToOleParameters(Parameter[] objParams)
        {
            OleDbParameter[] objSqlParams = null;
            if (objParams != null)
            {
                objSqlParams = new OleDbParameter[objParams.Length];
                for (int i = 0; i < objParams.Length; i++)
                {
                    objSqlParams[i] = new OleDbParameter(objParams[i].Name, Convert.ChangeType(objParams[i].Value, objParams[i].Type));
                }
            }
            return objSqlParams;
        }
        #endregion
    }
}
