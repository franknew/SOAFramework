﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.Core.Model
{
    /// <summary>
    /// 过滤器接口
    /// </summary>
    [ServiceLayer(Enabled = false)]
    public interface IFilter
    {
        /// <summary>
        /// 执行方法之前执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        bool OnActionExecuting(ActionContext context);

        /// <summary>
        /// 执行方法以后执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        bool OnActionExecuted(ActionContext context);

        void OnExceptionOccurs(ActionContext context);

        /// <summary>
        /// 错误信息
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// 是否给所有类公用
        /// </summary>
        bool GlobalUse { get; set; }

        /// <summary>
        /// 执行顺序
        /// </summary>
        int Index { get; set; }
    }

    /// <summary>
    /// 如果要使用过滤器，请继承该类
    /// </summary>
    [ServiceLayer(Enabled = false)]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class BaseFilter : Attribute, IFilter
    {
        /// <summary>
        /// 执行方法之前执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual bool OnActionExecuting(ActionContext context)
        {
            return true;
        }

        /// <summary>
        /// 执行方法以后执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual bool OnActionExecuted(ActionContext context)
        {
            return true;
        }

        public virtual void OnExceptionOccurs(ActionContext context)
        {

        }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 是否给所有类公用
        /// </summary>
        public bool GlobalUse { get; set; }

        public int Index { get; set; }
    }

    /// <summary>
    /// 如果要不执行过滤器，请在继承BaseFilter子类的基础上再继承该接口
    /// </summary>
    [ServiceLayer(Enabled = false)]
    public interface INoneExecuteFilter
    { }

    [ServiceLayer(Enabled = false)]
    [AttributeUsage(AttributeTargets.Class)]
    public class FilterAttribute: Attribute
    {
        public bool GlobalUse { get; set; }

        public int Index { get; set; }
    }
}