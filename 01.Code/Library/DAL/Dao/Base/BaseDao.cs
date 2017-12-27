﻿using IBatisNet.DataMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;

namespace SOAFramework.Library.DAL
{

    public partial class BaseDao<TEntity, TQueryForm, TUpdateForm> : IDao<TEntity, TQueryForm, TUpdateForm> where TEntity : BaseEntity
        where TQueryForm : BaseQueryForm where TUpdateForm : BaseUpdateForm<TEntity, TQueryForm>
    {
        private bool enableLog = false;
        public ISqlMapper Mapper { get; set; }

        internal string tableName = null;

        public BaseDao(ISqlMapper mapper = null)
        {
            if (mapper == null) this.Mapper = IBatisNet.DataMapper.Mapper.Instance();
            else this.Mapper = mapper;
            tableName = typeof(TEntity).Name;
            if (ConfigurationManager.AppSettings["EnableSqlLog"] != null && ConfigurationManager.AppSettings["EnableSqlLog"] == "1") enableLog = true;
        }

        public TEntity Add(TEntity entity)
        {
            return DaoHelper.Add<TEntity>(Mapper, entity);
        }

        public List<TEntity> Query(TQueryForm form)
        {
            return DaoHelper.Query<TEntity, TQueryForm>(Mapper, form, true);
        }

        public bool Delete(TQueryForm form)
        {
            string tableName = typeof(TEntity).Name;
            return DaoHelper.Delete<TQueryForm>(Mapper, tableName, form);
        }

        public bool Update(TUpdateForm form)
        {
            return DaoHelper.UpdateBaseEntity<TEntity, TQueryForm, TUpdateForm>(Mapper, form);
        }

        public int Count(string statementName, TQueryForm form)
        {
            return DaoHelper.GetCount<TQueryForm, TEntity>(Mapper, form, statementName, true);
        }

        public int Count(TQueryForm form)
        {
            return DaoHelper.GetCount<TQueryForm, TEntity>(Mapper, form);
        }

        public void WriteSqlLog(string statementName, object entity, ISqlMapSession session)
        {
            if (!enableLog) return;
            string sql = Mapper.GetRuntimeSql(statementName, entity, session);
            new SimpleLogger().Write(sql, true);
        }

        public TEntity QuerySingle(TQueryForm form)
        {
            return DaoHelper.QuerySingle<TEntity, TQueryForm>(Mapper, form);
        }
    }
}
