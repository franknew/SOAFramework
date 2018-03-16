using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace SOAFramework.Library.DAL
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
                if (value != null)
                {
                    obj_Type = value.GetType();
                    mStr_Value = value;
                }
                else
                {
                    obj_Type = DBNull.Value.GetType();
                    mStr_Value = DBNull.Value;
                }
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
        public static IDbDataParameter[] ChangeToParameters<T>(Parameter[] objParams) where T : IDbDataParameter
        {
            IDbDataParameter[] objSqlParams = null;
            if (objParams != null)
            {
                objSqlParams = new IDbDataParameter[objParams.Length];
                for (int i = 0; i < objParams.Length; i++)
                {
                    objSqlParams[i] = Activator.CreateInstance<T>();
                    objSqlParams[i].ParameterName = objParams[i].Name;
                    objSqlParams[i].Value =objParams[i].Value.ChangeTypeTo(objParams[i].Type);
                }
            }
            return objSqlParams;
        }

        public static IDbDataParameter[] ChangeToParameters(Parameter[] parameters, Type type)
        {
            IDbDataParameter[] objSqlParams = null;
            if (parameters != null)
            {
                objSqlParams = new IDbDataParameter[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    objSqlParams[i] = Activator.CreateInstance(type) as IDbDataParameter;
                    objSqlParams[i].ParameterName = parameters[i].Name;
                    objSqlParams[i].Value = parameters[i].Value.ChangeTypeTo(parameters[i].Type);
                }
            }
            return objSqlParams;
        }
        #endregion
    }
}
