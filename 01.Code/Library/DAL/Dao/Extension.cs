using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBatisNet.DataMapper;

namespace SOAFramework.Library.DAL
{
    public static class DaoExtension
    {
        public static string GetRuntimeSql(this ISqlMapper mapper, string statmenName, object entity)
        {
            string sql = "";
            var statment = mapper.GetMappedStatement(statmenName);
            if (!mapper.IsSessionStarted) mapper.OpenConnection(); 
            sql = statment.Statement.Sql.GetRequestScope(statment, entity, mapper.LocalSession).PreparedStatement.PreparedSql;
            return sql;
        }
    }
}
