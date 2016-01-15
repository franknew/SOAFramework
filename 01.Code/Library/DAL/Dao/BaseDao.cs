using IBatisNet.DataMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SOAFramework.Library.DAL
{

    public partial class BaseDao<TEngity, TQueryForm, TUpdateForm> where TEngity : BaseEntity
        where TQueryForm : BaseQueryForm where TUpdateForm : BaseUpdateForm<TEngity>
    {
        private bool enableLog = false;

        public ISqlMapper Mapper { get; set; }

        internal string tableName = null;

        public BaseDao(ISqlMapper mapper = null)
        {
            if (mapper == null)
            {
                this.Mapper = IBatisNet.DataMapper.Mapper.Instance();
            }
            else
            {
                this.Mapper = mapper;
            }
            tableName = typeof(TEngity).Name;
            if (ConfigurationManager.AppSettings["EnableSqlLog"] != null && ConfigurationManager.AppSettings["EnableSqlLog"] == "1") enableLog = true;
        }

        public string Add(TEngity entity)
        {
            if (string.IsNullOrEmpty(entity.ID))
            {
                entity.ID = Guid.NewGuid().ToString().Replace("-", "");
            }
            string action = "Add" + tableName;
            WriteSqlLog(action, entity);
            Mapper.Insert(action, entity);
            return entity.ID;
        }

        public List<TEngity> Query(TQueryForm form)
        {
            string action = "Query" + tableName;
            if (form.PageSize > 0)
            {
                WriteSqlLog(action + "RecordCount", form);
                int count = Mapper.QueryForObject<int>(action + "RecordCount", form);
                form.RecordCount = count;
            }
            WriteSqlLog(action, form);
            var list = Mapper.QueryForList<TEngity>(action, form).ToList();

            return list;
        }

        public bool Delete(TQueryForm form)
        {
            string action = "Delete" + tableName;
            WriteSqlLog(action, form);
            Mapper.Delete(action, form);
            return true;
        }

        public bool Update(TUpdateForm entity)
        {
            string action = "Update" + tableName;
            WriteSqlLog(action, entity);
            Mapper.Update(action, entity);
            return true;
        }


        private void WriteSqlLog(string statementName, object entity)
        {
            if (!enableLog) return;
            string sql = Mapper.GetRuntimeSql(statementName, entity);
            LogHelper.WriteLog(sql);
        }
    }
}
