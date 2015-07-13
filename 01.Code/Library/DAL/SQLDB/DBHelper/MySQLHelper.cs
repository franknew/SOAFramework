using SOAFramework.Library.DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Frank.Common.DAL
{
    public class MySQLHelper : BaseHelper<SqlConnection, SqlCommand, SqlDataAdapter, SqlParameter>
    {
        public MySQLHelper(string connectionstring) :
            base(new MySQLPagingSQL())
        {
            this.ConnectionString = connectionstring;
        }
    }
}
