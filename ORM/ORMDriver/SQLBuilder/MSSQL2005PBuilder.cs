using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SOAFramework.ORM.Mapping;

namespace SOAFramework.ORM.SQLBuilder
{
    public class MSSQL2005PBuilder : ISQLBuilder
    {
        public string BuildSelectColumnsSQL(Condition Condition)
        {
            string sql = "";
            return sql;
        }

        public string BuildWhereSQL()
        {
            string sql = "";
            return sql;
        }

        public string BuildGroupBySQL()
        {
            string sql = "";
            return sql;
        }

        public string BuildOrderBySQL()
        {
            string sql = "";
            return sql;
        }
    }
}
