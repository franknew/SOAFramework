using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace SOAFramework.Library.DAL
{
    public class OracleHelper : BaseHelper<OleDbConnection, OleDbCommand, OleDbDataAdapter, OleDbParameter>
    {
        public OracleHelper(string connectionstring)
        {
            this.ConnectionString = connectionstring;
        }
    }
}
