using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL
{
    public interface IPagingSQL
    {
        string GetPagingSQL(string SQL, string OrderBy, int StartIndex, int EndIndex);

        string GetCountSQL(string sql);
    }
}
