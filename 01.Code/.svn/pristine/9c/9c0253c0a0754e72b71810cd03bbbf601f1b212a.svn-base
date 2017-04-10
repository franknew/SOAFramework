using System;
using System.Collections.Generic;
using System.Text;

namespace Frank.Common.DAL
{
    public class DBFactory
    {
        public static DBHelper CreateDBHelper()
        {
            string strConn = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"];
            string strDBType = DBType.MSSQL.ToString();
            if (System.Configuration.ConfigurationSettings.AppSettings["DBType"] != null && System.Configuration.ConfigurationSettings.AppSettings["DBType"] != string.Empty)
            {
                strDBType = System.Configuration.ConfigurationSettings.AppSettings["DBType"];
            }
            return CreateDBHelper(strConn, strDBType);
        }

        public static DBHelper CreateDBHelper(string strConnectionString, string strDBType)
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
                default:
                    return CreateDBHelper(strConnectionString, DBType.MSSQL);
            }
        }

        public static DBHelper CreateDBHelper(DBType objType)
        {
            string strConn = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"];
            return CreateDBHelper(strConn, objType);
        }

        public static DBHelper CreateDBHelper(string strConn)
        {
            return CreateDBHelper(strConn, DBType.MSSQL);
        }

        public static DBHelper CreateDBHelper(string strConnectionString, DBType objType)
        {
            switch (objType)
            {
                case DBType.MSSQL:
                    MSSQLHelper objSQLCon = new MSSQLHelper(strConnectionString);
                    return (DBHelper)objSQLCon;
                case DBType.MSSQL2005P:
                    MSSQLHelper2005 objSQLCon2005P = new MSSQLHelper2005(strConnectionString);
                    return objSQLCon2005P;
                case DBType.Oracle:
                    OracleHelper objOracle = new OracleHelper(strConnectionString);
                    return (DBHelper)objOracle;
                //case DBType.MySQL:
                //    MySQLHelper objMySQL = new MySQLHelper(strConnectionString);
                //    return (DBHelper)objMySQL;
                case DBType.Access:
                    AccessHelper objAccess = new AccessHelper(strConnectionString);
                    return (DBHelper)objAccess;
                default:
                    MSSQLHelper2005 objSQLConTemp = new MSSQLHelper2005(strConnectionString);
                    return (DBHelper)objSQLConTemp;
            }
        }
    }

}
