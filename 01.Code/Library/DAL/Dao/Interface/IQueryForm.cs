using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL
{
    public interface IQueryForm
    {
        int? PageSize { get; set; }
        string OrderByColumn { get; set; }
        int? CurrentIndex { get; set; }
        int? StartIndex { get; }
        int? EndIndex { get; }
        int RecordCount { get; set; }
        int PageCount { get; }
        OrderBy OrderBy { get; set; }
    }
}
