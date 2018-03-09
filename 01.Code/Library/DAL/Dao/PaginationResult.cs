using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL
{
    public class PaginationResult<T> where T: IEntity
    {
        public PaginationResult(int recordCount)
        {
            RecordCount = recordCount;
        }

        public PaginationResult()
        { }

        public List<T> List { get; set; }
        public int RecordCount { get; set; }

    }
}
