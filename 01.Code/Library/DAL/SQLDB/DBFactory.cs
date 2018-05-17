using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;

namespace SOAFramework.Library.DAL
{
    public class DBFactory
    {
        private static bool logSql = false;

        public static IDBHelper CreateDBHelper()
        {
            string strDBType = DBType.MSSQL.ToString();
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["DBType"]))
            {
                strDBType = ConfigurationManager.AppSettings["DBType"];
            }
            else
            {
                strDBType = DBType.MSSQL.ToString();
            }
            string strConn = null;
            if (ConfigurationManager.ConnectionStrings[strDBType] != null && !string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings[strDBType].ConnectionString))
            {
                strConn = ConfigurationManager.ConnectionStrings[strDBType].ConnectionString;
            }
            if (string.IsNullOrEmpty(strConn))
            {
                throw new Exception("没有在ConnectionString配置节点配置连接字符串！");
            }
            var helper = CreateDBHelper(strConn, strDBType);
            return helper;
        }

        /// <summary>
        /// 根据connection string的key获取连接字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IDBHelper CreateDBHelperByKey(string key, DBType type)
        {
            string strDBType = type.ToString();
            bool logsql = false;
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["EnableSqlLog"]))
            {
                logsql = ConfigurationManager.AppSettings["EnableSqlLog"] == "1";
            }
            string strConn = null;
            if (ConfigurationManager.ConnectionStrings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings[key].ConnectionString))
            {
                strConn = ConfigurationManager.ConnectionStrings[key].ConnectionString;
            }
            if (string.IsNullOrEmpty(strConn))
            {
                throw new Exception("没有在ConnectionString配置节点配置连接字符串！");
            }
            var helper = CreateDBHelper(strConn, strDBType);
            return helper;
        }

        public static IDBHelper CreateDBHelper(string strConnectionString, string strDBType)
        {
            switch (strDBType.ToLower())
            {
                case "mssql":
                    return CreateDBHelper(strConnectionString, DBType.MSSQL);
                case "mssql2005p":
                    return CreateDBHelper(strConnectionString, DBType.MSSQL2005P);
                case "oracle":
                    return CreateDBHelper(strConnectionString, DBType.Oracle);
                case "access":
                    return CreateDBHelper(strConnectionString, DBType.Access);
                case "mysql":
                    return CreateDBHelper(strConnectionString, DBType.MySQL);
                case "sqlite":
                    return CreateDBHelper(strConnectionString, DBType.SQLite);
                case "db2":
                    return CreateDBHelper(strConnectionString, DBType.DB2);
                default:
                    return CreateDBHelper(strConnectionString, DBType.MSSQL);
            }
        }

        public static IDBHelper CreateDBHelper(DBType objType)
        {
            string strConn = ConfigurationManager.ConnectionStrings[objType.ToString()].ConnectionString;
            return CreateDBHelper(strConn, objType);
        }

        public static IDBHelper CreateDBHelper(string strConnectionString, DBType objType = DBType.MSSQL2005P)
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["EnableSqlLog"]))
            {
                logSql = ConfigurationManager.AppSettings["EnableSqlLog"] == "1";
            }
            string helperType = "";
            IDBHelper helper = null;
            string nameSpace = "";
            string connectionClassName = "";
            string commandClassName = "";
            string parameterClassName = "";
            string adapterClassName = "";
            string assemblyName = "";
            switch (objType)
            {
                case DBType.MSSQL:
                case DBType.MSSQL2005P:
                    nameSpace = "System.Data.SqlClient";
                    helperType = "MSSQLHelper";
                    connectionClassName = "SqlConnection";
                    commandClassName = "SqlCommand";
                    parameterClassName = "SqlParameter";
                    adapterClassName = "SqlDataAdapter";
                    assemblyName = "System.Data";
                    break;
                case DBType.Oracle:
                    //helper = new OracleHelper(strConnectionString);
                    helperType = "OracleHelper";
                    nameSpace = "System.Data.OracleClient";
                    connectionClassName = "OracleConnection";
                    commandClassName = "OracleCommand";
                    parameterClassName = "OracleParameter";
                    adapterClassName = "OracleDataAdapter";
                    assemblyName = "System.Data.OracleClient";
                    break;
                case DBType.MySQL:
                    //helper = new MySQLHelper(strConnectionString);
                    helperType = "MySQLHelper";
                    nameSpace = "MySql.Data.MySqlClient";
                    connectionClassName = "MySqlConnection";
                    commandClassName = "MySqlCommand";
                    parameterClassName = "MySqlParameter";
                    adapterClassName = "MySqlDataAdapter";
                    assemblyName = "MySql.Data";
                    break;
                case DBType.Access:
                    //helper = new AccessHelper(strConnectionString);
                    break;
                case DBType.SQLite:
                    //helper = new SQLiteHelper(strConnectionString);
                    helperType = "SQLiteHelper";
                    nameSpace = "System.Data.SQLite";
                    connectionClassName = "SQLiteConnection";
                    commandClassName = "SQLiteCommand";
                    parameterClassName = "SQLiteParameter";
                    adapterClassName = "SQLiteDataAdapter";
                    assemblyName = "System.Data.SqLite";
                    break;
                case DBType.DB2:
                    helperType = "DB2Helper";
                    nameSpace = "IBM.Data.DB2";
                    connectionClassName = "DB2Connection";
                    commandClassName = "DB2Command";
                    parameterClassName = "DB2Parameter";
                    adapterClassName = "DB2DataAdapter";
                    assemblyName = "IBM.Data.DB2";
                    break;
                default:
                    break;
            }
            Type type = Type.GetType("SOAFramework.Library.DAL." + helperType);
            var ass = AppDomain.CurrentDomain.GetAssembly(assemblyName);
            Type connectionType = ass.GetType(nameSpace + "." + connectionClassName);
            Type commandType = ass.GetType(nameSpace + "." + commandClassName);
            Type parameterType = ass.GetType(nameSpace + "." + parameterClassName);
            Type adapterType = ass.GetType(nameSpace + "." + adapterClassName);
            //helper = Activator.CreateInstance(type, strConnectionString) as IDBHelper;
            helper = new BaseHelperV2(connectionType, commandType, adapterType, parameterType, strConnectionString);
            helper.DBType = objType;
            helper.LogSql = logSql;
            return helper;
        }

        /// <summary>
        /// 创建分页器
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IPagingSQL CreateSqlPager(DBType type)
        {
            IPagingSQL paging = null;
            switch(type)
            {
                case DBType.MSSQL2005P:
                case DBType.MSSQL:
                    paging = new MSSQLPagingSQL();
                    break;
                case DBType.SQLite:
                    paging = new SQLitePagingSQL();
                    break;
                case DBType.Oracle:
                    paging = new OraclePagingSQL();
                    break;
            }
            return paging;
        }
    }

}
