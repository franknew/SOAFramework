using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

using Frank.Common.DAL;

namespace SOAFramework.ORM.Common
{
    public class ORMParameter
    {
        #region variables
        private object mStr_Value;
        private string mStr_Name;
        private Type obj_Type;
        #endregion

        #region constructor
        public ORMParameter(string strParameterName, object objValue)
        {
            mStr_Name = strParameterName;
            obj_Type = objValue.GetType();
            mStr_Value = objValue;
        }

        public ORMParameter(string strParameterName, object objValue, int intLength)
        {
            mStr_Name = strParameterName;;
            obj_Type = objValue.GetType();
            mStr_Value = objValue;
            if (objValue.ToString().Length >= intLength)
            {
                mStr_Value = objValue.ToString().Substring(0, intLength);
            }
        }

        public ORMParameter()
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
        public static List<Parameter> ToDALParameter(ORMParameter[] Parameters)
        {
            List<Parameter> lstParameter = new List<Parameter>();
            if (null != Parameters)
            {
                for (int i = 0;i < Parameters.Length; i++)
                {
                    Parameter objParam = new Parameter(Parameters[i].Name, Parameters[i].Value);
                    lstParameter.Add(objParam);
                }
            }
            return lstParameter;
        }

        public Parameter ToDALParameter()
        {
            Parameter objDALParameter = new Parameter(mStr_Name, mStr_Value);
            return objDALParameter;
        }
        #endregion
    }
}
