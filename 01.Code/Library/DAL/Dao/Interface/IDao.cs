using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL
{
    public interface IDao<TEntity, TQueryForm, TUpdateForm> where TEntity: IEntity
        where TQueryForm: IQueryForm
        where TUpdateForm: IUpdateForm
    {
        TEntity Add(TEntity entity);
        List<TEntity> Query(TQueryForm form);
        bool Delete(TQueryForm form);
        bool Update(TUpdateForm form);
        TEntity QuerySingle(TQueryForm form);
    }
}
