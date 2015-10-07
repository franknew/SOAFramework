using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.ORM.SQLBuilder
{
    public class SQLBuilderFactory
    {
        public static ISQLBuilder CreateBuilder(SQLBuilderType BuilderType)
        {
            ISQLBuilder builder;
            switch (BuilderType)
            {
                default:
                    builder = new MSSQL2005PBuilder();
                    break;
            }
            return builder;
        }
    }
}
