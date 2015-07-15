using IBatisNet.DataMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL
{
    public class SimpleDao<TEngity, TQueryForm, TUpdateForm> where TEngity : SimpleEntity
        where TUpdateForm : SimpleUpdateForm<TEngity>
    {
        public ISqlMapper Mapper { get; set; }

        internal string tableName = null;

        public SimpleDao(ISqlMapper mapper = null)
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
        }

        public string Add(TEngity entity)
        {
            if (string.IsNullOrEmpty(entity.ID))
            {
                entity.ID = Guid.NewGuid().ToString().Replace("-", "");
            }
            entity.CreateTime = DateTime.Now;
            entity.LastUpdateTime = DateTime.Now;
            Mapper.Insert("Add" + tableName, entity);
            return entity.ID;
        }

        public List<TEngity> Query(TQueryForm form)
        {
            return Mapper.QueryForList<TEngity>("Query" + tableName, form).ToList();
        }

        public bool Delete(TQueryForm form)
        {
            Mapper.Delete("Delete" + tableName, form);
            return true;
        }

        public bool Update(TUpdateForm entity)
        {
            entity.Entity.LastUpdateTime = DateTime.Now;
            Mapper.Update("Update" + tableName, entity);
            return true;
        }
    }
}
