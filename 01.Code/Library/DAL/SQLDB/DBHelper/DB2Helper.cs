
using IBM.Data.DB2;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL
{
    public class DB2Helper : BaseHelper<DB2Connection, DB2Command, DB2DataAdapter, DB2Parameter>
    {
        public DB2Helper(string connectionstring) :
            base(new MSSQLPagingSQL(), DBType.DB2)
        {
            this.ConnectionString = connectionstring;
        }
    }
}
