﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;

namespace SOAFramework.Library.DAL
{
    public class BaseHelperV2 : IDBHelper
    {
        public BaseHelperV2(Type connectionType, Type commandType, Type adapterType, Type parameterType, string connectionString)
        {
            _connectionType = connectionType;
            _commandType = commandType;
            _adapterType = adapterType;
            _paramterType = parameterType;
            mStr_ConnectionString = connectionString;
        }

        #region variables
        private SimpleLogger logger = new SimpleLogger();
        private string mStr_ConnectionString = "";
        private IPagingSQL paging = null;
        private DBType _type;
        private bool logSql = false;
        private Type _connectionType;
        private Type _commandType;
        private Type _adapterType;
        private Type _paramterType;
        #endregion

        #region properties
        public bool LogSql
        {
            get => logSql;
            set => logSql = value;
        }

        public string ConnectionString
        {
            set { mStr_ConnectionString = value; }
            get { return mStr_ConnectionString; }
        }

        public DBType DBType
        {
            get { return _type; }
            set { _type = value; }
        }

        public bool AutoCloseConnection { get; set; }

        public DBSuit CreateDBSuit<Con, Com, Adp>(ref IDbConnection connection, IDbCommand command)
            where Con : IDbConnection
            where Com : IDbCommand
            where Adp : IDbDataAdapter
        {
            DBSuit suit = new DBSuit();
            suit.Adapter = Activator.CreateInstance<Adp>();
            if (connection == null) suit.Conection = connection = Activator.CreateInstance<Con>();
            else suit.Conection = connection;
            if (command == null) suit.Command = command = Activator.CreateInstance<Com>();
            else suit.Command = command;
            return suit;
        }
        #endregion

        #region BeginTransaction
        public void BeginTransaction(string ConnectionString = null)
        {
        }
        #endregion

        #region Commit
        public void Commit()
        {
        }
        #endregion

        #region RollBack
        public void RollBack()
        {
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
        public DataTable GetTableWithSQL(string strCommandString, Parameter[] objParams, string strConnectionString, int intStartIndex, int intEndIndex, string strIDColumnName, OrderBy orderby)
        {
            StringBuilder strSQL = new StringBuilder();
            StringBuilder strSubSelect = new StringBuilder();
            DataTable dtData = new DataTable();
            DBSuit suite = DBSuit.CreateSuit(_connectionType, _commandType, _adapterType);
            IDbConnection objConnection = suite.Conection;
            IDbCommand objCommand = suite.Command;
            IDbDataAdapter objAdp = suite.Adapter;
            logger.Error(strConnectionString);
            if (intStartIndex > -1 && intEndIndex > 0 && strIDColumnName != string.Empty) { strSQL.Append(paging.GetPagingSQL(strCommandString, strIDColumnName, intStartIndex, intEndIndex, orderby)); }
            else strSQL.Append(strCommandString);
            logger.Error(strSQL.ToString());
            if (string.IsNullOrEmpty(strConnectionString)) strConnectionString = mStr_ConnectionString;
            if (string.IsNullOrEmpty(objConnection.ConnectionString)) objConnection.ConnectionString = strConnectionString;
            objCommand.CommandText = strSQL.ToString();
            objCommand.CommandType = CommandType.Text;
            objCommand.Connection = objConnection;
            IDbDataParameter[] parameters = Parameter.ChangeToParameters(objParams, _paramterType);
            if (parameters != null && parameters.Length > 0)
            {
                foreach (var p in parameters)
                {
                    objCommand.Parameters.Add(p);
                    logger.Error(p.ParameterName + ":" + p.Value.ToString());
                }
            }
            objAdp.SelectCommand = objCommand;
            try
            {
                if (objConnection.State != ConnectionState.Open && objConnection.State != ConnectionState.Connecting) objConnection.Open();
                DataSet set = new DataSet();
                if (logSql) { logger.Log(strCommandString);  }
                objAdp.Fill(set);
                if (set != null && set.Tables.Count > 0) dtData = set.Tables[0];
                return dtData;
            }
            finally
            {
                objConnection.Close();
            }
        }


        public DataTable GetTableWithSQL(string strCommandString, Parameter[] objParams, string strConnectionString, int intStartIndex, int intEndIndex, string strIDColumnName)
        {
            return this.GetTableWithSQL(strCommandString, objParams, strConnectionString, intStartIndex, intEndIndex, strIDColumnName, OrderBy.DESC);
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
            return GetTableWithSQL(strCommandString, objParams, mStr_ConnectionString, intStartIndex, intEndIndex, strIDColumnName, OrderBy.DESC);
        }

        public DataTable GetTableWithSQL(string strCommandString, Parameter[] objParams, int intStartIndex, int intEndIndex, string strIDColumnName, OrderBy orderBy)
        {
            return GetTableWithSQL(strCommandString, objParams, mStr_ConnectionString, intStartIndex, intEndIndex, strIDColumnName, orderBy);
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

        public DataTable GetTableWithSQL(string strCommandString, int intStartIndex, int intEndIndex, string strIDColumnName, OrderBy orderBy)
        {
            return GetTableWithSQL(strCommandString, null, mStr_ConnectionString, intStartIndex, intEndIndex, strIDColumnName, orderBy);
        }

        public DataTable GetTableWithSQL(string strCommandString, int intStartIndex, int intEndIndex, string strIDColumnName)
        {
            return GetTableWithSQL(strCommandString, null, mStr_ConnectionString, intStartIndex, intEndIndex, strIDColumnName, OrderBy.DESC);
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

        public DataTable GetTableWithSQL(string strCommandString, string strConnectionString, int intStartIndex, int intEndIndex, string strIDColumnName, OrderBy orderby)
        {
            return GetTableWithSQL(strCommandString, null, strConnectionString, intStartIndex, intEndIndex, strIDColumnName, orderby);
        }

        public DataTable GetTableWithSQL(string strCommandString, string strConnectionString, int intStartIndex, int intEndIndex, string strIDColumnName)
        {
            return GetTableWithSQL(strCommandString, null, strConnectionString, intStartIndex, intEndIndex, strIDColumnName, OrderBy.DESC);
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
            DBSuit suite = DBSuit.CreateSuit(_connectionType, _commandType, _adapterType);
            IDbConnection objConnection = suite.Conection;
            IDbCommand objCommand = suite.Command;
            IDbDataAdapter objAdp = suite.Adapter;
            if (string.IsNullOrEmpty(strConnectionString))
            {
                strConnectionString = mStr_ConnectionString;
            }
            if (objConnection.State != ConnectionState.Open && objConnection.State != ConnectionState.Connecting) objConnection.ConnectionString = strConnectionString; ;

            objCommand.CommandText = strSPName;
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.Connection = objConnection;
            IDbDataParameter[] parameters = Parameter.ChangeToParameters(objParams, _paramterType);
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
                if (objConnection.State != ConnectionState.Open && objConnection.State != ConnectionState.Connecting) objConnection.Open();
                DataSet set = new DataSet();
                objAdp.Fill(set);
                if (set != null && set.Tables.Count > 0) dtData = set.Tables[0];
                return dtData;
            }
            finally
            {
                objConnection.Close();
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
            DBSuit suite = DBSuit.CreateSuit(_connectionType, _commandType, _adapterType);
            IDbConnection objConnection = suite.Conection;
            IDbCommand objCommand = suite.Command;
            IDbDataAdapter objAdp = suite.Adapter;
            if (intStartIndex > -1 && intEndIndex > 0 && strIDColumnName != string.Empty) strSQL.Append(paging.GetPagingSQL(strCommandString, strIDColumnName, intStartIndex, intEndIndex));
            else strSQL.Append(strCommandString);
            DataSet dsData = new DataSet();
            if (string.IsNullOrEmpty(strConnectionString)) strConnectionString = mStr_ConnectionString;
            if (string.IsNullOrEmpty(objConnection.ConnectionString)) objConnection.ConnectionString = strConnectionString;

            objCommand.CommandText = strSQL.ToString();
            objCommand.CommandType = CommandType.Text;
            objCommand.Connection = objConnection;
            IDbDataParameter[] parameters = Parameter.ChangeToParameters(objParams, _paramterType);
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
                if (objConnection.State != ConnectionState.Open && objConnection.State != ConnectionState.Connecting) objConnection.Open();
                if (logSql) { logger.Log(strCommandString); }
                objAdp.Fill(dsData);
                return dsData;
            }
            finally
            {
                    objConnection.Close();
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
            DBSuit suite = DBSuit.CreateSuit(_connectionType, _commandType, _adapterType);
            IDbConnection objConnection = suite.Conection;
            IDbCommand objCommand = suite.Command;
            IDbDataAdapter objAdp = suite.Adapter;
            if (string.IsNullOrEmpty(strConnectionString))
            {
                strConnectionString = mStr_ConnectionString;
            }
            if (objConnection.State != ConnectionState.Open && objConnection.State != ConnectionState.Connecting) objConnection.ConnectionString = strConnectionString; ;

            objCommand.CommandText = strSPName;
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.Connection = objConnection;
            IDbDataParameter[] parameters = Parameter.ChangeToParameters(objParams, _paramterType);
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
                if (objConnection.State != ConnectionState.Open && objConnection.State != ConnectionState.Connecting) objConnection.Open();
                objAdp.Fill(dsData);
                return dsData;
            }
            finally
            {
                objConnection.Close();
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
            DBSuit suite = DBSuit.CreateSuit(_connectionType, _commandType, _adapterType);
            IDbConnection objConnection = suite.Conection;
            IDbCommand objCommand = suite.Command;
            IDbDataAdapter objAdp = suite.Adapter;
            if (string.IsNullOrEmpty(strConnectionString))
            {
                strConnectionString = mStr_ConnectionString;
            }
            if (string.IsNullOrEmpty(objConnection.ConnectionString)) objConnection.ConnectionString = mStr_ConnectionString;

            objCommand.CommandText = strCommandString;
            objCommand.CommandType = CommandType.Text;
            objCommand.Connection = objConnection;
            IDbDataParameter[] parameters = Parameter.ChangeToParameters(objParams, _paramterType);
            if (parameters != null && parameters.Length > 0)
            {
                foreach (var p in parameters)
                {
                    objCommand.Parameters.Add(p);
                }
            }

            try
            {
                if (objConnection.State != ConnectionState.Open && objConnection.State != ConnectionState.Connecting) objConnection.Open();
                if (logSql) { logger.Log(strCommandString); }
                objData = objCommand.ExecuteScalar();
                return objData;
            }
            finally
            {
                objConnection.Close();
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
            DBSuit suite = DBSuit.CreateSuit(_connectionType, _commandType, _adapterType);
            IDbConnection objConnection = suite.Conection;
            IDbCommand objCommand = suite.Command;
            IDbDataAdapter objAdp = suite.Adapter;
            if (string.IsNullOrEmpty(strConnectionString))
            {
                strConnectionString = mStr_ConnectionString;
            }
            if (objConnection.State != ConnectionState.Open && objConnection.State != ConnectionState.Connecting) objConnection.ConnectionString = strConnectionString; ;

            objCommand.CommandText = strSPName;
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.Connection = objConnection;
            IDbDataParameter[] parameters = Parameter.ChangeToParameters(objParams, _paramterType);
            if (parameters != null && parameters.Length > 0)
            {
                foreach (var p in parameters)
                {
                    objCommand.Parameters.Add(p);
                }
            }

            try
            {
                if (objConnection.State != ConnectionState.Open && objConnection.State != ConnectionState.Connecting) objConnection.Open();
                objData = objCommand.ExecuteScalar();
                return objData;
            }
            finally
            {
                objConnection.Close();
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
            DBSuit suite = DBSuit.CreateSuit(_connectionType, _commandType, _adapterType);
            IDbConnection objConnection = suite.Conection;
            IDbCommand objCommand = suite.Command;
            IDbDataAdapter objAdp = suite.Adapter;
            if (string.IsNullOrEmpty(strConnectionString)) strConnectionString = mStr_ConnectionString;
            if (string.IsNullOrEmpty(objConnection.ConnectionString)) objConnection.ConnectionString = mStr_ConnectionString;

            objCommand.CommandText = strCommandString;
            objCommand.CommandType = CommandType.Text;
            objCommand.Connection = objConnection;
            IDbDataParameter[] parameters = Parameter.ChangeToParameters(objParams, _paramterType);
            if (parameters != null && parameters.Length > 0)
            {
                foreach (var p in parameters)
                {
                    objCommand.Parameters.Add(p);
                }
            }
            try
            {
                if (objConnection.State != ConnectionState.Open && objConnection.State != ConnectionState.Connecting) objConnection.Open();
                if (logSql) { logger.Log(strCommandString); }
                return objCommand.ExecuteNonQuery();
            }
            finally
            {
                objConnection.Close();
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
            DBSuit suite = DBSuit.CreateSuit(_connectionType, _commandType, _adapterType);
            IDbConnection objConnection = suite.Conection;
            IDbCommand objCommand = suite.Command;
            IDbDataAdapter objAdp = suite.Adapter;
            if (string.IsNullOrEmpty(strConnectionString))
            {
                strConnectionString = mStr_ConnectionString;
            }
            if (objConnection.State != ConnectionState.Open && objConnection.State != ConnectionState.Connecting) objConnection.ConnectionString = strConnectionString; ;

            objCommand.CommandText = strSPName;
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.Connection = objConnection;
            IDbDataParameter[] parameters = Parameter.ChangeToParameters(objParams, _paramterType);
            if (parameters != null && parameters.Length > 0)
            {
                foreach (var p in parameters)
                {
                    objCommand.Parameters.Add(p);
                }
            }

            try
            {
                if (objConnection.State != ConnectionState.Open && objConnection.State != ConnectionState.Connecting) objConnection.Open();
                return objCommand.ExecuteNonQuery();
            }
            finally
            {
                objConnection.Close();
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

        public void CloseConnection()
        {
        }
        #endregion
    }
}