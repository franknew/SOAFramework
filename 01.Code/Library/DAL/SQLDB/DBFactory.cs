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
            if (ConfigurationManager.AppSettings["DBType"] != null && ConfigurationManager.AppSettings["DBType"] != string.Empty)
            {
                strDBType = ConfigurationManager.AppSettings["DBType"];
            }
            string strConn = ConfigurationManager.ConnectionStrings[strDBType].ConnectionString;
            return CreateDBHelper(strConn, strDBType);
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
            return CreateDBHelper(strConn, DBType.MSSQL);
        }

        public static IDBHelper CreateDBHelper(string strConnectionString, DBType objType)
        {
            IDBHelper helper = null;
            switch (objType)
            {
                case DBType.MSSQL:
                    helper = new MSSQLHelper(strConnectionString);
                    break;
                case DBType.MSSQL2005P:
                    helper = new MSSQLHelper(strConnectionString);
                    break;
                case DBType.Oracle:
                    helper = new OracleHelper(strConnectionString);
                    break;
                //case DBType.MySQL:
                //    MySQLHelper objMySQL = new MySQLHelper(strConnectionString);
                //    return (DBHelper)objMySQL;
                case DBType.Access:
                    //helper = new AccessHelper(strConnectionString);
                    break;
                case DBType.SQLite:
                    helper = new SQLiteHelper(strConnectionString);
                    break;
                default:
                    helper = new MSSQLHelper(strConnectionString);
                    break;
            }
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
