using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;

namespace SOAFramework.Library.DAL
{
    public class MSSQLHelper : BaseHelper<SqlConnection, SqlCommand, SqlDataAdapter, SqlParameter>
    {
        public MSSQLHelper(string connectionstring) :
            base(new MSSQLPagingSQL())
        {
            this.ConnectionString = connectionstring;
        }
    }
}
