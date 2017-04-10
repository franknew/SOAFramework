using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL
{
    public abstract class BaseHelper<Con, Com, Adp, Param> : IDBHelper
        where Con : IDbConnection
        where Com : IDbCommand
        where Adp : IDbDataAdapter
        where Param : IDbDataParameter
    {
        public BaseHelper(IPagingSQL paging)
        {
            this.paging = paging;
        }

        #region variables
        private string mStr_ConnectionString = "";
        private IDbCommand mObj_Command = null;
        private IDbConnection mObj_Connection = null;
        private bool mBl_IsTransaction = false;
        private IPagingSQL paging = null;
        #endregion

        #region properties
        public string ConnectionString
        {
            set { mStr_ConnectionString = value; }
            get { return mStr_ConnectionString; }
        }

        public DBType DBType
        {
            get { return DBType.MSSQL2005P; }
        }

        public DBSuit CreateDBSuit<Con, Com, Adp>(IDbConnection connection, IDbCommand command)
            where Con : IDbConnection
            where Com : IDbCommand
            where Adp : IDbDataAdapter
        {
            DBSuit suit = new DBSuit();
            suit.Adapter = Activator.CreateInstance<Adp>();
            if (connection == null)
            {
                suit.Conection = Activator.CreateInstance<Con>();
            }
            else
            {
                suit.Conection = connection;
            }
            if (command == null)
            {
                suit.Command = Activator.CreateInstance<Com>();
            }
            else
            {
                suit.Command = command;
            }
            return suit;
        }
        #endregion

        #region BeginTransaction
        public void BeginTransaction(string ConnectionString = null)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                ConnectionString = mStr_ConnectionString;
            }
            mObj_Command = new SqlCommand();
            mObj_Connection = new SqlConnection(ConnectionString);
            mObj_Connection.Open();
            mObj_Connection.BeginTransaction();
            mBl_IsTransaction = true;
        }
        #endregion

        #region Commit
        public void Commit()
        {
            if (mObj_Connection != null && mObj_Command != null)
            {
                mObj_Command.Transaction.Commit();
                mObj_Connection.Close();
            }
            mBl_IsTransaction = false;
        }
        #endregion

        #region RollBack
        public void RollBack()
        {
            if (mObj_Connection != null && mObj_Command != null)
            {
                mObj_Command.Transaction.Rollback();
                mObj_Connection.Close();
            }
            mBl_IsTransaction = false;
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
            DataTable dtData = new DataTable();
            DBSuit suite = CreateDBSuit<Con, Com, Adp>(mObj_Connection, mObj_Command);
            IDbConnection objConnection = suite.Conection;
            IDbCommand objCommand = suite.Command;
            IDbDataAdapter objAdp = suite.Adapter;
            if (intStartIndex > -1 && intEndIndex > 0 && strIDColumnName != string.Empty)
            {
                strSQL.Append(paging.GetPagingSQL(strCommandString, strIDColumnName, intStartIndex, intEndIndex));
            }
            else
            {
                strSQL.Append(strCommandString);
            }
            if (string.IsNullOrEmpty(strConnectionString))
            {
                strConnectionString = mStr_ConnectionString;
            }
            objConnection.ConnectionString = strConnectionString;
            objCommand.CommandText = strSQL.ToString();
            objCommand.CommandType = CommandType.Text;
            objCommand.Connection = objConnection;
            IDbDataParameter[] parameters = Parameter.ChangeToParameters<Param>(objParams);
            if (parameters != null && parameters.Length > 0)
            {
                foreach (var p in parameters)
                {
                    objCommand.Parameters.Add(p);
                }
            }
            objAdp.SelectCommand = objCommand;

            try
            {
                if (objConnection.State != ConnectionState.Open)
                {
                    objConnection.Open();
                }
                DataSet set = new DataSet();
                objAdp.Fill(set);
                if (set != null && set.Tables.Count > 0)
                {
                    dtData = set.Tables[0];
                }
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
            DBSuit suite = CreateDBSuit<Con, Com, Adp>(mObj_Connection, mObj_Command);
            IDbConnection objConnection = suite.Conection;
            IDbCommand objCommand = suite.Command;
            IDbDataAdapter objAdp = suite.Adapter;
            if (string.IsNullOrEmpty(strConnectionString))
            {
                strConnectionString = mStr_ConnectionString;
            }
            objConnection.ConnectionString = strConnectionString;

            objCommand.CommandText = strSPName;
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.Connection = objConnection;
            IDbDataParameter[] parameters = Parameter.ChangeToParameters<Param>(objParams);
            if (parameters != null && parameters.Length > 0)
            {
                foreach (var p in parameters)
                {
                    objCommand.Parameters.Add(p);
                }
            }
            objAdp.SelectCommand = objCommand;

            try
            {
                if (objConnection.State != ConnectionState.Open)
                {
                    objConnection.Open();
                }
                DataSet set = new DataSet();
                objAdp.Fill(set);
                if (set != null && set.Tables.Count > 0)
                {
                    dtData = set.Tables[0];
                }
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
            DBSuit suite = CreateDBSuit<Con, Com, Adp>(mObj_Connection, mObj_Command);
            IDbConnection objConnection = suite.Conection;
            IDbCommand objCommand = suite.Command;
            IDbDataAdapter objAdp = suite.Adapter;
            if (intStartIndex > -1 && intEndIndex > 0 && strIDColumnName != string.Empty)
            {
                strSQL.Append(paging.GetPagingSQL(strCommandString, strIDColumnName, intStartIndex, intEndIndex));
            }
            else
            {
                strSQL.Append(strCommandString);
            }
            DataSet dsData = new DataSet();
            if (string.IsNullOrEmpty(strConnectionString))
            {
                objConnection.ConnectionString = strConnectionString;
            }

            objCommand.CommandText = strSQL.ToString();
            objCommand.CommandType = CommandType.Text;
            objCommand.Connection = objConnection;
            IDbDataParameter[] parameters = Parameter.ChangeToParameters<Param>(objParams);
            if (parameters != null && parameters.Length > 0)
            {
                foreach (var p in parameters)
                {
                    objCommand.Parameters.Add(p);
                }
            }
            objAdp.SelectCommand = objCommand;

            try
            {
                if (objConnection.State != ConnectionState.Open)
                {
                    objConnection.Open();
                }
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
            DBSuit suite = CreateDBSuit<Con, Com, Adp>(mObj_Connection, mObj_Command);
            IDbConnection objConnection = suite.Conection;
            IDbCommand objCommand = suite.Command;
            IDbDataAdapter objAdp = suite.Adapter;
            if (string.IsNullOrEmpty(strConnectionString))
            {
                strConnectionString = mStr_ConnectionString;
            }
            objConnection.ConnectionString = strConnectionString;

            objCommand.CommandText = strSPName;
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.Connection = objConnection;
            IDbDataParameter[] parameters = Parameter.ChangeToParameters<Param>(objParams);
            if (parameters != null && parameters.Length > 0)
            {
                foreach (var p in parameters)
                {
                    objCommand.Parameters.Add(p);
                }
            }
            objAdp.SelectCommand = objCommand;

            try
            {
                if (objConnection.State != ConnectionState.Open)
                {
                    objConnection.Open();
                }
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
            DBSuit suite = CreateDBSuit<Con, Com, Adp>(mObj_Connection, mObj_Command);
            IDbConnection objConnection = suite.Conection;
            IDbCommand objCommand = suite.Command;
            IDbDataAdapter objAdp = suite.Adapter;
            if (string.IsNullOrEmpty(strConnectionString))
            {
                strConnectionString = mStr_ConnectionString;
            }
            objConnection.ConnectionString = strConnectionString;

            objCommand.CommandText = strCommandString;
            objCommand.CommandType = CommandType.Text;
            objCommand.Connection = objConnection;
            IDbDataParameter[] parameters = Parameter.ChangeToParameters<Param>(objParams);
            if (parameters != null && parameters.Length > 0)
            {
                foreach (var p in parameters)
                {
                    objCommand.Parameters.Add(p);
                }
            }

            try
            {
                if (objConnection.State != ConnectionState.Open)
                {
                    objConnection.Open();
                }
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
            DBSuit suite = CreateDBSuit<Con, Com, Adp>(mObj_Connection, mObj_Command);
            IDbConnection objConnection = suite.Conection;
            IDbCommand objCommand = suite.Command;
            IDbDataAdapter objAdp = suite.Adapter;
            if (string.IsNullOrEmpty(strConnectionString))
            {
                strConnectionString = mStr_ConnectionString;
            }
            objConnection.ConnectionString = strConnectionString;

            objCommand.CommandText = strSPName;
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.Connection = objConnection;
            IDbDataParameter[] parameters = Parameter.ChangeToParameters<Param>(objParams);
            if (parameters != null && parameters.Length > 0)
            {
                foreach (var p in parameters)
                {
                    objCommand.Parameters.Add(p);
                }
            }

            try
            {
                if (objConnection.State != ConnectionState.Open)
                {
                    objConnection.Open();
                }
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
            DBSuit suite = CreateDBSuit<Con, Com, Adp>(mObj_Connection, mObj_Command);
            IDbConnection objConnection = suite.Conection;
            IDbCommand objCommand = suite.Command;
            IDbDataAdapter objAdp = suite.Adapter;
            if (string.IsNullOrEmpty(strConnectionString))
            {
                strConnectionString = mStr_ConnectionString;
            }
            objConnection.ConnectionString = strConnectionString;

            objCommand.CommandText = strCommandString;
            objCommand.CommandType = CommandType.Text;
            objCommand.Connection = objConnection;
            IDbDataParameter[] parameters = Parameter.ChangeToParameters<Param>(objParams);
            if (parameters != null && parameters.Length > 0)
            {
                foreach (var p in parameters)
                {
                    objCommand.Parameters.Add(p);
                }
            }
            try
            {
                if (objConnection.State != ConnectionState.Open)
                {
                    objConnection.Open();
                }
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
            DBSuit suite = CreateDBSuit<Con, Com, Adp>(mObj_Connection, mObj_Command);
            IDbConnection objConnection = suite.Conection;
            IDbCommand objCommand = suite.Command;
            IDbDataAdapter objAdp = suite.Adapter;
            if (string.IsNullOrEmpty(strConnectionString))
            {
                strConnectionString = mStr_ConnectionString;
            }
            objConnection.ConnectionString = strConnectionString;

            objCommand.CommandText = strSPName;
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.Connection = objConnection;
            IDbDataParameter[] parameters = Parameter.ChangeToParameters<Param>(objParams);
            if (parameters != null && parameters.Length > 0)
            {
                foreach (var p in parameters)
                {
                    objCommand.Parameters.Add(p);
                }
            }

            try
            {
                if (objConnection.State != ConnectionState.Open)
                {
                    objConnection.Open();
                }
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

    }
}
