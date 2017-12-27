using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL
{
    public interface IQueryForm
    {
        /// <summary>
        /// 页长
        /// </summary>
        int? PageSize { get; set; }
        /// <summary>
        /// 排序列
        /// </summary>
        string OrderByColumn { get; set; }
        /// <summary>
        /// 当前页数
        /// </summary>
        int? CurrentIndex { get; set; }
        /// <summary>
        /// 查询起始索引
        /// </summary>
        int? StartIndex { get; }
        /// <summary>
        /// 查询结束索引
        /// </summary>
        int? EndIndex { get; }
        /// <summary>
        /// 数据总量
        /// </summary>
        int RecordCount { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        int PageCount { get; }
        /// <summary>
        /// 排序类型
        /// </summary>
        OrderBy OrderBy { get; set; }
    }
}
