using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace SOAFramework.Library.DAL
{
    public class DBFactory
    {
        public static IDBHelper CreateDBHelper()
        {
            string strDBType = DBType.MSSQL.ToString();
            bool logsql = false;
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["EnableSqlLog"]))
            {
                logsql = ConfigurationManager.AppSettings["EnableSqlLog"] == "1";
            }
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
            helper.LogSql = logsql;
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

        public static IDBHelper CreateDBHelper(string strConn)
        {
            return CreateDBHelper(strConn, DBType.MSSQL2005P);
        }

        public static IDBHelper CreateDBHelper(string strConnectionString, DBType objType)
        {
            string helperType = "";
            IDBHelper helper = null;
            switch (objType)
            {
                case DBType.MSSQL:
                    //helper = new MSSQLHelper(strConnectionString);
                    helperType = "MSSQLHelper";
                    break;
                case DBType.MSSQL2005P:
                    //helper = new MSSQLHelper(strConnectionString);
                    helperType = "MSSQLHelper";
                    break;
                case DBType.Oracle:
                    //helper = new OracleHelper(strConnectionString);
                    helperType = "OracleHelper";
                    break;
                case DBType.MySQL:
                    //helper = new MySQLHelper(strConnectionString);
                    helperType = "MySQLHelper";
                    break;
                case DBType.Access:
                    //helper = new AccessHelper(strConnectionString);
                    break;
                case DBType.SQLite:
                    //helper = new SQLiteHelper(strConnectionString);
                    helperType = "SQLiteHelper";
                    break;
                case DBType.DB2:
                    helperType = "DB2Helper";
                    break;
                default:
                    //helper = new MSSQLHelper(strConnectionString);
                    helperType = "MSSQLHelper";
                    break;
            }
            Type type = Type.GetType("SOAFramework.Library.DAL." + helperType);
            helper = Activator.CreateInstance(type, strConnectionString) as IDBHelper;
            return helper;
        }

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
