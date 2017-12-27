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
        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="TEntity">实体类</typeparam>
        /// <typeparam name="TQueryForm">查询类</typeparam>
        /// <param name="mapper">mapper</param>
        /// <param name="form">参数</param>
        /// <param name="enableLog">是否写sql日志</param>
        /// <returns></returns>
        public static List<TEntity> Query<TEntity, TQueryForm>(ISqlMapper mapper, TQueryForm form, bool enableLog = true) where TEntity : IEntity
            where TQueryForm : IQueryForm
        {
            string tableName = typeof(TEntity).Name;
            string action = tableName + ".Query";
            return QueryForList<TEntity, TQueryForm>(mapper, action, form, enableLog);
        }

        /// <summary>
        /// 查询出列表
        /// </summary>
        /// <typeparam name="TEntity">实体类</typeparam>
        /// <typeparam name="TQueryForm">查询类</typeparam>
        /// <param name="mapper">mapper</param>
        /// <param name="statementName">sql名称</param>
        /// <param name="form">参数</param>
        /// <param name="enableLog">是否写sql日志</param>
        /// <returns></returns>
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

        /// <summary>
        /// 获取总数
        /// </summary>
        /// <typeparam name="TQueryForm"></typeparam>
        /// <param name="mapper"></param>
        /// <param name="session"></param>
        /// <param name="command"></param>
        /// <param name="statementName"></param>
        /// <param name="form"></param>
        /// <param name="enableLog"></param>
        /// <returns></returns>
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
                logger.Write(sql, true);
                logger.Write(paramString, true);
            }
            return count;
        }

        /// <summary>
        /// 获取总数
        /// </summary>
        /// <typeparam name="TQueryForm"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="mapper"></param>
        /// <param name="form"></param>
        /// <param name="statementName"></param>
        /// <param name="enableLog"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TQueryForm"></typeparam>
        /// <param name="mapper"></param>
        /// <param name="session"></param>
        /// <param name="command"></param>
        /// <param name="statementName"></param>
        /// <param name="form"></param>
        /// <param name="enableLog"></param>
        /// <returns></returns>
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
                logger.Write(sql, true);
                logger.Write(paramString, true);
            }
            return list;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="mapper"></param>
        /// <param name="entity"></param>
        /// <param name="enableLog"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TQueryForm"></typeparam>
        /// <typeparam name="TUpdateForm"></typeparam>
        /// <param name="mapper"></param>
        /// <param name="form"></param>
        /// <param name="enableLog"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TQueryForm"></typeparam>
        /// <typeparam name="TUpdateForm"></typeparam>
        /// <param name="mapper"></param>
        /// <param name="form"></param>
        /// <param name="enableLog"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TQueryForm"></typeparam>
        /// <param name="mapper"></param>
        /// <param name="tableName"></param>
        /// <param name="form"></param>
        /// <param name="enableLog"></param>
        /// <returns></returns>
        public static bool Delete<TQueryForm>(ISqlMapper mapper, string tableName, TQueryForm form, bool enableLog = true) where TQueryForm : IQueryForm
        {
            string action = tableName + ".Delete";
            if (enableLog) WriteSqlLog(mapper, action, form, mapper.GetSession());
            mapper.Delete(action, form);
            return true;
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="statementName"></param>
        /// <param name="entity"></param>
        /// <param name="session"></param>
        public static void WriteSqlLog(ISqlMapper mapper, string statementName, object entity, ISqlMapSession session)
        {
            string sql = mapper.GetRuntimeSql(statementName, entity, session);
            new SimpleLogger().Write(sql, true);
        }

        /// <summary>
        /// 查询第一个
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TQueryForm"></typeparam>
        /// <param name="mapper"></param>
        /// <param name="form"></param>
        /// <param name="enableLog"></param>
        /// <returns></returns>
        public static TEntity QuerySingle<TEntity, TQueryForm>(ISqlMapper mapper, TQueryForm form, bool enableLog = true) where TEntity : IEntity
            where TQueryForm : IQueryForm
        {
            var list = Query<TEntity, TQueryForm>(mapper, form, enableLog);
            if (list == null || list.Count == 0) return default(TEntity);
            return list[0];
        }

        /// <summary>
        /// 组装参数
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="statmentName"></param>
        /// <param name="form"></param>
        /// <param name="command"></param>
        /// <returns></returns>
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
