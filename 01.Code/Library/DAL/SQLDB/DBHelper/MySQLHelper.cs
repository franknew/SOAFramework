using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL
{
    public class MySQLHelper : BaseHelper<MySqlConnection, MySqlCommand, MySqlDataAdapter, MySqlParameter>
    {
        public MySQLHelper(string connectionstring) :
            base(new MySQLPagingSQL())
        {
            this.ConnectionString = connectionstring;
        }
    }
}
