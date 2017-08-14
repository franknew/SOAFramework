using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.OracleClient;

namespace SOAFramework.Library.DAL
{
    public class OracleHelper : BaseHelper<OracleConnection, OracleCommand, OracleDataAdapter, OracleParameter>
    {

        public OracleHelper(string connectionstring)
            : base(new OraclePagingSQL(), DBType.Oracle)
        {
            this.ConnectionString = connectionstring;
        }
    }
}
