using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Frank.Common.DAL
{
    public class MSSQLHelper : DBHelper
    {
         
        #region variables
        private string mStr_ConnectionString = "";
        private SqlCommand mObj_Command = null;
        private SqlConnection mObj_Connection = null;
        private bool mBl_IsTransaction = false;
        #endregion

        #region constructor
        public MSSQLHelper(string strConnectionString)
        {
            mStr_ConnectionString = strConnectionString;
        }
        #endregion

        #region BeginTransaction
        public void BeginTransaction(string ConnectionString)
        {
            mObj_Command = new SqlCommand();
            mObj_Connection = new SqlConnection(ConnectionString);
            mObj_Connection.BeginTransaction();
            mBl_IsTransaction = true;
        }

        public void BeginTransaction()
        {
            BeginTransaction(mStr_ConnectionString);
        }
        #endregion

        #region Commit
        public void Commit()
        {
            if (mObj_Connection != null && mObj_Command != null)
            {
                mObj_Command.Transaction.Commit();
                mObj_Connection.Close();
                mBl_IsTransaction = false;
            }
        }
        #endregion

        #region RollBack
        public void RollBack()
        {
            if (mObj_Connection != null && mObj_Command != null)
            {
                mObj_Command.Transaction.Rollback();
                mObj_Connection.Close();
                mBl_IsTransaction = false;
            }
        }
        #endregion

        #region GetTableWithSQL
        /// <summary>
        /// get a datatable from database with sql string
        /// </summary>
        /// <param name="strCommandString">select string</param>
        /// <param name="objParams">parameters</param>
        /// <param name="strConnectionString">connection string</param>
        /// <returns>data</returns>
        public DataTable GetTableWithSQL(string strCommandString, Parameter[] objParams, string strConnectionString, int intStartIndex, int intEndIndex, string strIDColumnName)
        {
            StringBuilder strSQL = new StringBuilder();
            StringBuilder strSubSelect = new StringBuilder();
            strSQL.Append(strCommandString);
            if (intStartIndex > -1 && intEndIndex > 0 && strIDColumnName != string.Empty)
            {
                #region construct subselect string
                strSubSelect.Append(strCommandString);
                int intIndexSubSelect = strSubSelect.ToString().ToLower().IndexOf("select ") + 7;
                int intIndexSubFrom = strSubSelect.ToString().ToLower().IndexOf(" from ");
                string strTopExclude = " top " + intStartIndex.ToString() + " " + strIDColumnName + " ";
                strSubSelect.Remove(intIndexSubSelect, intIndexSubFrom - intIndexSubSelect);
                strSubSelect.Insert(intIndexSubSelect, strTopExclude);
                #endregion

                #region construct main select string
                int intIndexWhere = strCommandString.ToLower().IndexOf(" where ");
                int intRecordCount = intEndIndex - intStartIndex;
                string strTopN = " top " + intRecordCount.ToString() + " ";
                int intIndexOfSelect = strCommandString.ToLower().IndexOf("select ") + 7;

                strSQL.Insert(intIndexOfSelect, strTopN);
                int intIndexOrderBy = strSQL.ToString().ToLower().IndexOf(" order ");
                if (intIndexWhere > 0)
                {
                    if (intIndexOrderBy > 0)
                    {
                        strSQL.Insert(intIndexOrderBy, " and " + strIDColumnName + " not in (" + strSubSelect.ToString() + ")");
                    }
                    else
                    {
                        strSQL.Append(" and ");
                        strSQL.Append(strIDColumnName + " not in (");
                        strSQL.Append(strSubSelect.ToString());
                        strSQL.Append(")");
                    }
                }
                else
                {
                    if (intIndexOrderBy > 0)
                    {
                        strSQL.Insert(intIndexOrderBy, " where " + strIDColumnName + " not in (" + strSubSelect.ToString() + ")");
                    }
                    else
                    {
                        strSQL.Append(" where ");
                        strSQL.Append(strIDColumnName + " not in (");
                        strSQL.Append(strSubSelect.ToString());
                        strSQL.Append(")");
                    }
                }
                #endregion
            }
            DataTable dtData = new DataTable();
            SqlConnection objConnection = new SqlConnection();
            if (mObj_Connection != null)
            {
                objConnection = mObj_Connection;
            }
            SqlCommand objCommand = new SqlCommand();
            if (mObj_Command != null)
            {
                objCommand = mObj_Command;
            }
            SqlDataAdapter objAdp = new SqlDataAdapter();

            objConnection.ConnectionString = strConnectionString;
            objCommand.CommandText = strSQL.ToString();
            objCommand.CommandType = CommandType.Text;
            objCommand.Connection = objConnection;
            if (objParams != null && objParams.Length > 0)
            {
                objCommand.Parameters.AddRange(Parameter.ChangeToSqlParameters(objParams));;
            }
            objAdp.SelectCommand = objCommand;

            try
            {
                objConnection.Open();
                objAdp.Fill(dtData);
                return dtData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!mBl_IsTransaction)
                {
                    objConnection.Close();
                }
            }
        }

        public DataTable GetTableWithSQL(string strCommandString, Parameter[] objParams, string strConnectionString)
        {
            return GetTableWithSQL(strCommandString, objParams, strConnectionString, 0, 0, "");
        }

        /// <summary>
        /// get a datatable from database with sql string
        /// </summary>
        /// <param name="strCommandString">select string</param>
        /// <param name="objParams">parameters</param>
        /// <returns>data</returns>
        public DataTable GetTableWithSQL(string strCommandString, Parameter[] objParams)
        {
            return GetTableWithSQL(strCommandString, objParams, mStr_ConnectionString, 0, 0, "");
        }

        public DataTable GetTableWithSQL(string strCommandString, Parameter[] objParams, int intStartIndex, int intEndIndex, string strIDColumnName)
        {
            return GetTableWithSQL(strCommandString, objParams, mStr_ConnectionString, intStartIndex, intEndIndex, strIDColumnName);
        }

        /// <summary>
        /// get a datatable from database with sql string
        /// </summary>
        /// <param name="strCommandString">select string</param>
        /// <returns>data</returns>
        public DataTable GetTableWithSQL(string strCommandString)
        {
            return GetTableWithSQL(strCommandString, null, mStr_ConnectionString, 0, 0, "");
        }

        public DataTable GetTableWithSQL(string strCommandString, int intStartIndex, int intEndIndex, string strIDColumnName)
        {
            return GetTableWithSQL(strCommandString, null, mStr_ConnectionString, intStartIndex, intEndIndex, strIDColumnName);
        }

        /// <summary>
        /// get a datatable from database with sql string
        /// </summary>
        /// <param name="strCommandString">select string</param>
        /// <param name="strConnectionString">connection string</param>
        /// <returns>data</returns>
        public DataTable GetTableWithSQL(string strCommandString, string strConnectionString)
        {
            return GetTableWithSQL(strCommandString, null, strConnectionString, 0, 0, "");
        }

        public DataTable GetTableWithSQL(string strCommandString, string strConnectionString, int intStartIndex, int intEndIndex, string strIDColumnName)
        {
            return GetTableWithSQL(strCommandString, null, strConnectionString, intStartIndex, intEndIndex, strIDColumnName);
        }
        #endregion

        #region GetTableWithSP

        /// <summary>
        /// get a datatable from database with SP
        /// </summary>
        /// <param name="strSPName">SP name</param>
        /// <param name="objParams">parameters</param>
        /// <param name="strConnectionString">connection string</param>
        /// <returns>data</returns>
        public DataTable GetTableWithSP(string strSPName, Parameter[] objParams, string strConnectionString)
        {
            DataTable dtData = new DataTable();
            SqlConnection objConnection = new SqlConnection();
            if (mObj_Connection != null)
            {
                objConnection = mObj_Connection;
            }
            SqlCommand objCommand = new SqlCommand();
            if (mObj_Command != null)
            {
                objCommand = mObj_Command;
            }
            SqlDataAdapter objAdp = new SqlDataAdapter();

            objConnection.ConnectionString = strConnectionString;
            objCommand.CommandText = strSPName;
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.Connection = objConnection;
            if (objParams != null && objParams.Length > 0)
            {
                objCommand.Parameters.AddRange(Parameter.ChangeToSqlParameters(objParams));
            }
            objAdp.SelectCommand = objCommand;

            try
            {
                objConnection.Open();
                objAdp.Fill(dtData);
                return dtData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!mBl_IsTransaction)
                {
                    objConnection.Close();
                }
            }
        }

        /// <summary>
        /// get a datatable from database with SP
        /// </summary>
        /// <param name="strSPName">SP name</param>
        /// <param name="objParams">parameters</param>
        /// <returns>data</returns>
        public DataTable GetTableWithSP(string strSPName, Parameter[] objParams)
        {
            return GetTableWithSP(strSPName, objParams, mStr_ConnectionString);
        }

        /// <summary>
        /// get a datatable from database with SP
        /// </summary>
        /// <param name="strSPName">SP name</param>
        /// <param name="strConnectionString">connection string</param>
        /// <returns>data</returns>
        public DataTable GetTableWithSP(string strSPName, string strConnectionString)
        {
            return GetTableWithSP(strSPName, null, strConnectionString);
        }

        /// <summary>
        /// get a datatable from database with SP
        /// </summary>
        /// <param name="strSPName">SP name</param>
        /// <returns>data</returns>
        public DataTable GetTableWithSP(string strSPName)
        {
            return GetTableWithSP(strSPName, null, mStr_ConnectionString);
        }
        #endregion

        #region GetDataSetWithSQL

        /// <summary>
        /// get a dataset from database with SQL string
        /// </summary>
        /// <param name="strCommandString">command string</param>
        /// <param name="objParams">parameters</param>
        /// <param name="strConnectionString">connection string</param>
        /// <returns>data</returns>
        public DataSet GetDataSetWithSQL(string strCommandString, Parameter[] objParams, string strConnectionString, int intStartIndex, int intEndIndex, string strIDColumnName)
        {
            StringBuilder strSQL = new StringBuilder();
            StringBuilder strSubSelect = new StringBuilder();
            strSQL.Append(strCommandString);
            if (intStartIndex > -1 && intEndIndex > 0 && strIDColumnName != string.Empty)
            {
                #region construct subselect string
                strSubSelect.Append(strCommandString);
                int intIndexSubSelect = strSubSelect.ToString().ToLower().IndexOf("select ") + 7;
                int intIndexSubFrom = strSubSelect.ToString().ToLower().IndexOf(" from ");
                string strTopExclude = " top " + intStartIndex.ToString() + " " + strIDColumnName + " ";
                strSubSelect.Remove(intIndexSubSelect, intIndexSubFrom - intIndexSubSelect);
                strSubSelect.Insert(intIndexSubSelect, strTopExclude);
                #endregion

                #region construct main select string
                int intIndexWhere = strCommandString.ToLower().IndexOf(" where ");
                int intRecordCount = intEndIndex - intStartIndex;
                string strTopN = " top " + intRecordCount.ToString() + " ";
                int intIndexOfSelect = strCommandString.ToLower().IndexOf("select ") + 7;

                strSQL.Insert(intIndexOfSelect, strTopN);
                int intIndexOrderBy = strSQL.ToString().ToLower().IndexOf(" order ");
                if (intIndexWhere > 0)
                {
                    if (intIndexOrderBy > 0)
                    {
                        strSQL.Insert(intIndexOrderBy, " and " + strIDColumnName + " not in (" + strSubSelect.ToString() + ")");
                    }
                    else
                    {
                        strSQL.Append(" and ");
                        strSQL.Append(strIDColumnName + " not in (");
                        strSQL.Append(strSubSelect.ToString());
                        strSQL.Append(")");
                    }
                }
                else
                {
                    if (intIndexOrderBy > 0)
                    {
                        strSQL.Insert(intIndexOrderBy, " where " + strIDColumnName + " not in (" + strSubSelect.ToString() + ")");
                    }
                    else
                    {
                        strSQL.Append(" where ");
                        strSQL.Append(strIDColumnName + " not in (");
                        strSQL.Append(strSubSelect.ToString());
                        strSQL.Append(")");
                    }
                }
                #endregion
            }
            DataSet dsData = new DataSet();
            SqlConnection objConnection = new SqlConnection();
            if (mObj_Connection != null)
            {
                objConnection = mObj_Connection;
            }
            SqlCommand objCommand = new SqlCommand();
            if (mObj_Command != null)
            {
                objCommand = mObj_Command;
            }
            SqlDataAdapter objAdp = new SqlDataAdapter();

            objConnection.ConnectionString = strConnectionString;
            objCommand.CommandText = strSQL.ToString();
            objCommand.CommandType = CommandType.Text;
            objCommand.Connection = objConnection;
            if (objParams != null && objParams.Length > 0)
            {
                objCommand.Parameters.AddRange(Parameter.ChangeToSqlParameters(objParams));;
            }
            objAdp.SelectCommand = objCommand;

            try
            {
                objConnection.Open();
                objAdp.Fill(dsData);
                return dsData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!mBl_IsTransaction)
                {
                    objConnection.Close();
                }
            }
        }

        public DataSet GetDataSetWithSQL(string strCommandString, Parameter[] objParams, string strConnectionString)
        {
            return GetDataSetWithSQL(strCommandString, objParams, strConnectionString, 0, 0, "");
        }

        /// <summary>
        /// get a dataset from database with SQL string
        /// </summary>
        /// <param name="strCommandString">command string</param>
        /// <param name="objParams">parameters</param>
        /// <returns>data</returns>
        public DataSet GetDataSetWithSQL(string strCommandString, Parameter[] objParams)
        {
            return GetDataSetWithSQL(strCommandString, objParams, mStr_ConnectionString, 0, 0, "");
        }

        public DataSet GetDataSetWithSQL(string strCommandString, Parameter[] objParams, int intStartIndex, int intEndIndex, string strIDColumnName)
        {
            return GetDataSetWithSQL(strCommandString, objParams, mStr_ConnectionString, intStartIndex, intEndIndex, strIDColumnName);
        }

        /// <summary>
        /// get a dataset from database with SQL string
        /// </summary>
        /// <param name="strCommandString">command string</param>
        /// <param name="strConnectionString">connection string</param>
        /// <returns>data</returns>
        public DataSet GetDataSetWithSQL(string strCommandString, string strConnectionString)
        {
            return GetDataSetWithSQL(strCommandString, null, strConnectionString, 0, 0, "");
        }

        public DataSet GetDataSetWithSQL(string strCommandString, string strConnectionString, int intStartIndex, int intEndIndex, string strIDColumnName)
        {
            return GetDataSetWithSQL(strCommandString, null, strConnectionString, intStartIndex, intEndIndex, strIDColumnName);
        }

        /// <summary>
        /// get a dataset from database with SQL string
        /// </summary>
        /// <param name="strCommandString">command string</param>
        /// <returns>data</returns>
        public DataSet GetDataSetWithSQL(string strCommandString)
        {
            return GetDataSetWithSQL(strCommandString, null, mStr_ConnectionString, 0, 0, "");
        }

        public DataSet GetDataSetWithSQL(string strCommandString, int intStartIndex, int intEndIndex, string strIDColumnName)
        {
            return GetDataSetWithSQL(strCommandString, null, mStr_ConnectionString, intStartIndex, intEndIndex, strIDColumnName);
        }
        #endregion

        #region GetDataSetWithSP

        /// <summary>
        /// get a dataset from database with SP
        /// </summary>
        /// <param name="strSPName">SP name</param>
        /// <param name="objParams">parameters</param>
        /// <param name="strConnectionString">connection string</param>
        /// <returns>data</returns>
        public DataSet GetDataSetWithSP(string strSPName, Parameter[] objParams, string strConnectionString)
        {
            DataSet dsData = new DataSet();
            SqlConnection objConnection = new SqlConnection();
            if (mObj_Connection != null)
            {
                objConnection = mObj_Connection;
            }
            SqlCommand objCommand = new SqlCommand();
            if (mObj_Command != null)
            {
                objCommand = mObj_Command;
            }
            SqlDataAdapter objAdp = new SqlDataAdapter();

            objConnection.ConnectionString = strConnectionString;
            objCommand.CommandText = strSPName;
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.Connection = objConnection;
            if (objParams != null && objParams.Length > 0)
            {
                objCommand.Parameters.AddRange(Parameter.ChangeToSqlParameters(objParams));;
            }
            objAdp.SelectCommand = objCommand;

            try
            {
                objConnection.Open();
                objAdp.Fill(dsData);
                return dsData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!mBl_IsTransaction)
                {
                    objConnection.Close();
                }
            }
        }

        /// <summary>
        /// get a dataset from database with SP
        /// </summary>
        /// <param name="strSPName">SP name</param>
        /// <param name="objParams">parameters</param>
        /// <returns>data</returns>
        public DataSet GetDataSetWithSP(string strSPName, Parameter[] objParams)
        {
            return GetDataSetWithSP(strSPName, objParams, mStr_ConnectionString);
        }

        /// <summary>
        /// get a dataset from database with SP
        /// </summary>
        /// <param name="strSPName">SP name</param>
        /// <param name="strConnectionString">connection string</param>
        /// <returns>data</returns>
        public DataSet GetDataSetWithSP(string strSPName, string strConnectionString)
        {
            return GetDataSetWithSP(strSPName, null, strConnectionString);
        }

        /// <summary>
        /// get a dataset from database with SP
        /// </summary>
        /// <param name="strSPName">SP name</param>
        /// <returns>data</returns>
        public DataSet GetDataSetWithSP(string strSPName)
        {
            return GetDataSetWithSP(strSPName, null, mStr_ConnectionString);
        }
        #endregion

        #region GetScalarWithSQL
        /// <summary>
        /// get an object from database with SQL string
        /// </summary>
        /// <param name="strCommandString">command string</param>
        /// <param name="objParams">parameters</param>
        /// <param name="strConnectionString">connection string</param>
        /// <returns>data</returns>
        public object GetScalarWithSQL(string strCommandString, Parameter[] objParams, string strConnectionString)
        {
            object objData = null;
            SqlConnection objConnection = new SqlConnection();
            SqlCommand objCommand = new SqlCommand();
            if (mObj_Command != null)
            {
                objCommand = mObj_Command;
            }
            SqlDataAdapter objAdp = new SqlDataAdapter();

            objConnection.ConnectionString = strConnectionString;
            objCommand.CommandText = strCommandString;
            objCommand.CommandType = CommandType.Text;
            objCommand.Connection = objConnection;
            if (objParams != null && objParams.Length > 0)
            {
                objCommand.Parameters.AddRange(Parameter.ChangeToSqlParameters(objParams));;
            }

            try
            {
                objConnection.Open();
                objData = objCommand.ExecuteScalar();
                return objData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!mBl_IsTransaction)
                {
                    objConnection.Close();
                }
            }
        }

        /// <summary>
        /// get an object from database with SQL string
        /// </summary>
        /// <param name="strCommandString">command string</param>
        /// <param name="objParams">parameters</param>
        /// <returns>data</returns>
        public object GetScalarWithSQL(string strCommandString, Parameter[] objParams)
        {
            return GetScalarWithSQL(strCommandString, objParams, mStr_ConnectionString);
        }

        /// <summary>
        /// get an object from database with SQL string
        /// </summary>
        /// <param name="strCommandString">command string</param>
        /// <param name="strConnectionString">connection string</param>
        /// <returns>data</returns>
        public object GetScalarWithSQL(string strCommandString, string strConnectionString)
        {
            return GetScalarWithSQL(strCommandString, null, strConnectionString);
        }

        /// <summary>
        /// get an object from database with SQL string
        /// </summary>
        /// <param name="strCommandString">command string</param>
        /// <returns>data</returns>
        public object GetScalarWithSQL(string strCommandString)
        {
            return GetScalarWithSQL(strCommandString, null, mStr_ConnectionString);
        }
        #endregion

        #region GetScalarWithSP
        /// <summary>
        /// get an object from database with SP
        /// </summary>
        /// <param name="strSPName">SP name</param>
        /// <param name="objParams">parameters</param>
        /// <param name="strConnectionString">connection string</param>
        /// <returns>data</returns>
        public object GetScalarWithSP(string strSPName, Parameter[] objParams, string strConnectionString)
        {
            object objData = null;
            SqlConnection objConnection = new SqlConnection();
            SqlCommand objCommand = new SqlCommand();
            if (mObj_Command != null)
            {
                objCommand = mObj_Command;
            }
            SqlDataAdapter objAdp = new SqlDataAdapter();

            objConnection.ConnectionString = strConnectionString;
            objCommand.CommandText = strSPName;
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.Connection = objConnection;
            if (objParams != null && objParams.Length > 0)
            {
                objCommand.Parameters.AddRange(Parameter.ChangeToSqlParameters(objParams));;
            }

            try
            {
                objConnection.Open();
                objData = objCommand.ExecuteScalar();
                return objData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!mBl_IsTransaction)
                {
                    objConnection.Close();
                }
            }
        }

        /// <summary>
        /// get an object from database with SP
        /// </summary>
        /// <param name="strSPName">SP name</param>
        /// <param name="objParams">parameters</param>
        /// <returns>data</returns>
        public object GetScalarWithSP(string strSPName, Parameter[] objParams)
        {
            return GetScalarWithSP(strSPName, objParams, mStr_ConnectionString);
        }

        /// <summary>
        /// get an object from database with SP
        /// </summary>
        /// <param name="strSPName">SP name</param>
        /// <param name="strConnectionString">connection string</param>
        /// <returns>data</returns>
        public object GetScalarWithSP(string strSPName, string strConnectionString)
        {
            return GetScalarWithSP(strSPName, null, strConnectionString);
        }

        /// <summary>
        /// get an object from database with SP
        /// </summary>
        /// <param name="strSPName">SP name</param>
        /// <returns>data</returns>
        public object GetScalarWithSP(string strSPName)
        {
            return GetScalarWithSP(strSPName, null, mStr_ConnectionString);
        }
        #endregion

        #region ExecNoneQueryWithSQL
        public int ExecNoneQueryWithSQL(string strCommandString, Parameter[] objParams, string strConnectionString)
        {
            SqlConnection objConnection = new SqlConnection();
            if (mObj_Connection != null)
            {
                objConnection = mObj_Connection;
            }
            SqlCommand objCommand = new SqlCommand();
            if (mObj_Command != null)
            {
                objCommand = mObj_Command;
            }
            SqlDataAdapter objAdp = new SqlDataAdapter();

            objConnection.ConnectionString = strConnectionString;
            objCommand.CommandText = strCommandString;
            objCommand.CommandType = CommandType.Text;
            objCommand.Connection = objConnection;
            if (objParams != null && objParams.Length > 0)
            {
                objCommand.Parameters.AddRange(Parameter.ChangeToSqlParameters(objParams));;
            }
            try
            {
                objConnection.Open();
                return objCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (objCommand.Transaction != null)
                {
                    objCommand.Transaction.Rollback();
                }
                throw ex;
            }
            finally
            {
                if (!mBl_IsTransaction)
                {
                    objConnection.Close();
                }
            }
        }

        public int ExecNoneQueryWithSQL(string strCommandString, Parameter[] objParams)
        {
            return ExecNoneQueryWithSQL(strCommandString, objParams, mStr_ConnectionString);
        }

        public int ExecNoneQueryWithSQL(string strCommandString, string strConnectionString)
        {
            return ExecNoneQueryWithSQL(strCommandString, null, strConnectionString);
        }

        public int ExecNoneQueryWithSQL(string strCommandString)
        {
            return ExecNoneQueryWithSQL(strCommandString, null, mStr_ConnectionString);
        }
        #endregion

        #region ExecNoneQueryWithSP
        public int ExecNoneQueryWithSP(string strSPName, Parameter[] objParams, string strConnectionString)
        {
            SqlConnection objConnection = new SqlConnection();
            if (mObj_Connection != null)
            {
                objConnection = mObj_Connection;
            }
            SqlCommand objCommand = new SqlCommand();
            if (mObj_Command != null)
            {
                objCommand = mObj_Command;
            }
            SqlDataAdapter objAdp = new SqlDataAdapter();

            objConnection.ConnectionString = strConnectionString;
            objCommand.CommandText = strSPName;
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.Connection = objConnection;
            if (objParams != null && objParams.Length > 0)
            {
                objCommand.Parameters.AddRange(Parameter.ChangeToSqlParameters(objParams));;
            }

            try
            {
                objConnection.Open();
                return objCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!mBl_IsTransaction)
                {
                    objConnection.Close();
                }
            }
        }

        public int ExecNoneQueryWithSP(string strSPName, Parameter[] objParams)
        {
            return ExecNoneQueryWithSP(strSPName, objParams, mStr_ConnectionString);
        }

        public int ExecNoneQueryWithSP(string strSPName, string strConnectionString)
        {
            return ExecNoneQueryWithSP(strSPName, null, strConnectionString);
        }

        public int ExecNoneQueryWithSP(string strSPName)
        {
            return ExecNoneQueryWithSP(strSPName, null, mStr_ConnectionString);
        }
        #endregion

        #region attributes
        public string ConnectionString
        {
            get { return mStr_ConnectionString; }
        }

        public DBType DBType
        {
            get { return DBType.MSSQL; }
        }
        #endregion

    }
}
