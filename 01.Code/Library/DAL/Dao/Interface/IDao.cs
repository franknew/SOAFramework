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
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Add(TEntity entity);
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        List<TEntity> Query(TQueryForm form);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        bool Delete(TQueryForm form);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        bool Update(TUpdateForm form);
        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        TEntity QuerySingle(TQueryForm form);
    }
}
