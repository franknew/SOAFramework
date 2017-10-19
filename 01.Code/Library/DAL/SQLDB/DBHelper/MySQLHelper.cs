using MySql.Data.MySqlClient;
using SOAFramework.Library.DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace SOAFramework.Library.DAL
{
    public class MySQLHelper : BaseHelper<MySqlConnection, MySqlCommand, MySqlDataAdapter, MySqlParameter>
    {
        public MySQLHelper(string connectionstring) :
            base(new MySQLPagingSQL(), DBType.MySQL)
        {
            this.ConnectionString = connectionstring;
        }
    }
}
