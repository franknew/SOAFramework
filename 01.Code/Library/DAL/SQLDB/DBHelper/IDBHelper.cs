using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SOAFramework.Library.DAL
{
    public interface IDBHelper
    {
        #region Attributes
        /// <summary>
        /// �����ַ���
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// ���ݿ�����
        /// </summary>
        DBType DBType { get; }

        bool Lockable { get; set; }

        bool AutoCloseConnection { get; set; }

        bool LogSql { get; set; }
        #endregion

        #region BeginTransaction
        /// <summary>
        /// ���ÿ�ʼ����
        /// </summary>
        /// <param name="ConnectionString"></param>
        void BeginTransaction(string ConnectionString = null);

        void CloseConnection();
        #endregion

        #region Commit
        /// <summary>
        /// �ύ����
        /// </summary>
        void Commit();
        #endregion

        #region RollBack
        /// <summary>
        /// �ع�����
        /// </summary>
        void RollBack();
        #endregion

        #region GetTableWithSQL
        DataTable GetTableWithSQL(string strCommandString, string strConnectionString);

        DataTable GetTableWithSQL(string strCommandString, string strConnectionString, int intStartIndex, int intEndIndex, string strIDColumnName);

        DataTable GetTableWithSQL(string strCommandString);

        DataTable GetTableWithSQL(string strCommandString, int intStartIndex, int intEndIndex, string strIDColumnName);

        DataTable GetTableWithSQL(string strCommandString, Parameter[] objParams);

        DataTable GetTableWithSQL(string strCommandString, Parameter[] objParams, int intStartIndex, int intEndIndex, string strIDColumnName);

        DataTable GetTableWithSQL(string strCommandString, Parameter[] objParams, string strConnectionString);

        DataTable GetTableWithSQL(string strCommandString, Parameter[] objParams, string strConnectionString, int intStartIndex, int intEndIndex, string strIDColumnName);
        #endregion

        #region GetTableWithSP
        DataTable GetTableWithSP(string strSPName, string strConnectionString);

        DataTable GetTableWithSP(string strSPName);

        DataTable GetTableWithSP(string strSPName, Parameter[] objParams);

        DataTable GetTableWithSP(string strSPName, Parameter[] objParams, string strConnectionString);
        #endregion

        #region GetDataSetWithSQL
        DataSet GetDataSetWithSQL(string strCommandString, string strConnectionString);

        DataSet GetDataSetWithSQL(string strCommandString, string strConnectionString, int intStartIndex, int intLastIndex, string strIDColumnName);

        DataSet GetDataSetWithSQL(string strCommandString, int intStartIndex, int intLastIndex, string strIDColumnName);

        DataSet GetDataSetWithSQL(string strCommandString);

        DataSet GetDataSetWithSQL(string strCommandString, Parameter[] objParams);

        DataSet GetDataSetWithSQL(string strCommandString, Parameter[] objParams, string strConnectionString);

        DataSet GetDataSetWithSQL(string strCommandString, Parameter[] objParams, string strConnectionString, int intStartIndex, int intLastIndex, string strIDColumnName);

        DataSet GetDataSetWithSQL(string strCommandString, Parameter[] objParams, int intStartIndex, int intLastIndex, string strIDColumnName);
        #endregion

        #region GetDataSetWithSP
        DataSet GetDataSetWithSP(string strSPName, string strConnectionString);

        DataSet GetDataSetWithSP(string strSPName);

        DataSet GetDataSetWithSP(string strSPName, Parameter[] objParams);

        DataSet GetDataSetWithSP(string strSPName, Parameter[] objParams, string strConnectionString);
        #endregion

        #region GetScalarWithSQL
        object GetScalarWithSQL(string strCommandString, string strConnectionString);

        object GetScalarWithSQL(string strCommandString);

        object GetScalarWithSQL(string strCommandString, Parameter[] objParams);

        object GetScalarWithSQL(string strCommandString, Parameter[] objParams, string strConnectionString);
        #endregion

        #region GetScalarWithSP
        object GetScalarWithSP(string strSPName, string strConnectionString);

        object GetScalarWithSP(string strSPName);

        object GetScalarWithSP(string strSPName, Parameter[] objParams);

        object GetScalarWithSP(string strSPName, Parameter[] objParams, string strConnectionString);
        #endregion

        #region ExecNoneQueryWithSQL
        int ExecNoneQueryWithSQL(string strCommandString, string strConnectionString);

        int ExecNoneQueryWithSQL(string strCommandString);

        int ExecNoneQueryWithSQL(string strCommandString, Parameter[] objParams);

        int ExecNoneQueryWithSQL(string strCommandString, Parameter[] objParams, string strConnectionString);
        #endregion

        #region ExecNoneQueryWithSP
        int ExecNoneQueryWithSP(string strSPName, string strConnectionString);

        int ExecNoneQueryWithSP(string strSPName);

        int ExecNoneQueryWithSP(string strSPName, Parameter[] objParams);

        int ExecNoneQueryWithSP(string strSPName, Parameter[] objParams, string strConnectionString);
        #endregion
    }

    public class DBSuit
    {
        public IDbCommand Command { get; set; }

        public IDbConnection Conection { get; set; }

        public IDbDataAdapter Adapter { get; set; }
    }
}
