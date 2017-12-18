using IBatisNet.DataMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SOAFramework.Library.DAL
{
    public class DaoHelper
    {
        public static List<TEntity> Query<TEntity, TQueryForm>(ISqlMapper mapper, TQueryForm form, bool enableLog = true) where TEntity : IEntity
            where TQueryForm : IQueryForm
        {
            string tableName = typeof(TEntity).Name;
            string action = tableName + ".Query";
            return QueryForList<TEntity, TQueryForm>(mapper, action, form, enableLog);
        }

        public static List<TEntity> QueryForList<TEntity, TQueryForm>(ISqlMapper mapper,
            string statementName,
            TQueryForm form,
            bool enableLog = true)
            where TEntity : IEntity
            where TQueryForm : IQueryForm
        {
            string tableName = typeof(TEntity).Name;
            var list = new List<TEntity>();
            var session = mapper.GetSession();
            if (form.PageSize > 0)
            {
                try
                {
                    IDbCommand command = session.CreateCommand(CommandType.Text);
                    //query count
                    int count = GetCount(mapper, session, command, statementName, form, enableLog);
                    //query paging
                    form.RecordCount = count;
                    list = QueryForPaging<TEntity, TQueryForm>(mapper, session, command, statementName, form, enableLog);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    //if (session.Connection.State == ConnectionState.Open) session.Connection.Close();
                }
            }
            else
            {
                IDbCommand command = session.CreateCommand(CommandType.Text);
                list = QueryForPaging<TEntity, TQueryForm>(mapper, session, command, statementName, form, enableLog);
            }

            return list;
        }

        public static int GetCount<TQueryForm>(ISqlMapper mapper, ISqlMapSession session, IDbCommand command, string statementName, TQueryForm form, bool enableLog = true)
            where TQueryForm : IQueryForm
        {
            IPagingSQL paging = PagingSQLFactory.Create(mapper.DataSource.DbProvider.Name);
            var sql = mapper.GetRuntimeSql(statementName, form, session);
            var countsql = paging.GetCountSQL(sql);
            command.Connection = session.Connection;
            command.CommandText = countsql;
            var paramString = BuildParams(mapper, statementName, form, command);
            if (command.Connection.State != ConnectionState.Open) command.Connection.Open();
            int count = (int)command.ExecuteScalar();
            if (enableLog)
            {
                SimpleLogger logger = new SimpleLogger();
                logger.Write(sql);
                logger.Write(paramString);
            }
            return count;
        }

        public static int GetCount<TQueryForm, TEntity>(ISqlMapper mapper, TQueryForm form, string statementName = null, bool enableLog = true)
            where TQueryForm : IQueryForm
            where TEntity: IEntity
        {
            var session = mapper.GetSession();
            IDbCommand command = session.CreateCommand(CommandType.Text);
            if (string.IsNullOrEmpty(statementName))
            {
                string tableName = typeof(TEntity).Name;
                statementName = tableName + ".Query";
            }
            return GetCount<TQueryForm>(mapper, session, command, statementName, form, enableLog);
        }

        public static List<TEntity> QueryForPaging<TEntity, TQueryForm>(ISqlMapper mapper,
            ISqlMapSession session,
            IDbCommand command,
            string statementName,
            TQueryForm form,
            bool enableLog = true)
            where TEntity : IEntity
            where TQueryForm : IQueryForm
        {
            IPagingSQL paging = PagingSQLFactory.Create(mapper.DataSource.DbProvider.Name);
            var sql = mapper.GetRuntimeSql(statementName, form, session);

            if (form.StartIndex > -1 && form.EndIndex > -1)
            {
                sql = paging.GetPagingSQL(sql, form.OrderByColumn, form.StartIndex.Value, form.EndIndex.Value, form.OrderBy);
            }
            sql = paging.BuildOrderBy(sql, form.OrderByColumn, form.OrderBy);
            command.CommandText = sql;
            string paramString = BuildParams(mapper, statementName, form, command);

            IDataAdapter dataAdapter = session.CreateDataAdapter(command);

            DataSet set = new DataSet();
            dataAdapter.Fill(set);
            DataTable table = set.Tables[0];
            var list = table.ToList<TEntity>().ToList();
            if (enableLog)
            {
                SimpleLogger logger = new SimpleLogger();
                logger.Write(sql);
                logger.Write(paramString);
            }
            return list;
        }

        public static TEntity Add<TEntity>(ISqlMapper mapper, TEntity entity, bool enableLog = true) where TEntity : IEntity
        {
            var type = typeof(TEntity);
            string tableName = type.Name;
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            DateTime now = DateTime.Now;
            foreach (var property in properties)
            {
                var primarykey = property.GetCustomAttribute<PrimaryKey>(true);
                if (primarykey != null)
                {
                    object value = entity.TryGetValue(property.Name);
                    if (value == null)
                    {
                        IIDGenerator generator = IDGeneratorFactory.Create(GeneratorType.SnowFlak);
                        value = generator.Generate();
                        entity.TrySetValue(property.Name, value);
                    }
                }
                if ((property.Name.ToLower().Equals("createtime") || property.Name.ToLower().Equals("lastupdatetime")) && (property.PropertyType.Equals(typeof(DateTime)) || property.PropertyType.Equals(typeof(Nullable<DateTime>))))
                {
                    entity.TrySetValue(property.Name, now);
                }
            }
            string action = tableName + ".Add";
            if (enableLog) WriteSqlLog(mapper, action, entity, mapper.GetSession());
            mapper.Insert(action, entity);
            return entity;
        }

        public static bool UpdateSimpleEntity<TEntity, TQueryForm, TUpdateForm>(ISqlMapper mapper, TUpdateForm form, bool enableLog = true) where TEntity : SimpleEntity
            where TQueryForm : SimpleQueryForm
            where TUpdateForm : SimpleUpdateForm<TEntity, TQueryForm>
        {
            string tableName = typeof(TEntity).Name;
            string action = tableName + ".Update";
            form.Entity.LastUpdateTime = DateTime.Now;
            if (enableLog) WriteSqlLog(mapper, action, form, mapper.GetSession());
            mapper.Update(action, form);
            return true;
        }

        public static bool UpdateBaseEntity<TEntity, TQueryForm, TUpdateForm>(ISqlMapper mapper, TUpdateForm form, bool enableLog = true) where TEntity : BaseEntity
            where TQueryForm : BaseQueryForm
            where TUpdateForm : BaseUpdateForm<TEntity, TQueryForm>
        {
            string tableName = typeof(TEntity).Name;
            string action = tableName + ".Update";
            if (enableLog) WriteSqlLog(mapper, action, form, mapper.GetSession());
            mapper.Update(action, form);
            return true;
        }

        public static bool Delete<TQueryForm>(ISqlMapper mapper, string tableName, TQueryForm form, bool enableLog = true) where TQueryForm : IQueryForm
        {
            string action = tableName + ".Delete";
            if (enableLog) WriteSqlLog(mapper, action, form, mapper.GetSession());
            mapper.Delete(action, form);
            return true;
        }

        public static void WriteSqlLog(ISqlMapper mapper, string statementName, object entity, ISqlMapSession session)
        {
            string sql = mapper.GetRuntimeSql(statementName, entity, session);
            new SimpleLogger().Write(sql);
        }

        public static TEntity QuerySingle<TEntity, TQueryForm>(ISqlMapper mapper, TQueryForm form, bool enableLog = true) where TEntity : IEntity
            where TQueryForm : IQueryForm
        {
            var list = Query<TEntity, TQueryForm>(mapper, form, enableLog);
            if (list == null || list.Count == 0) return default(TEntity);
            return list[0];
        }

        public static string BuildParams(ISqlMapper mapper, string statmentName, object form, IDbCommand command)
        {
            command.Parameters.Clear();
            StringBuilder builder = new StringBuilder();
            var session = mapper.GetSession();
            var statment = mapper.GetMappedStatement(statmentName);
            var requestScope = statment.Statement.Sql.GetRequestScope(statment, form, session);
            for (int i = 0; i < requestScope.ParameterMap.PropertiesList.Count; i++)
            {
                var property = requestScope.ParameterMap.GetProperty(i);
                //requestScope.ParameterMap.SetParameter(property, command.CreateParameter(), form);
                string parameterName = string.Empty;
                parameterName = "param" + i.ToString();
                if (session.DataSource.DbProvider.UseParameterPrefixInParameter) parameterName = session.DataSource.DbProvider.ParameterPrefix + parameterName;
                object value = form.TryGetValue(property.PropertyName, false);
                var parameter = session.CreateDataParameter();
                parameter.ParameterName = parameterName;
                parameter.Value = value;
                command.Parameters.Add(parameter);
                builder.AppendFormat("{0}:{1} ", parameterName, value.ToString());
            }
            return builder.ToString();
        }

    }
}
