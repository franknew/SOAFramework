using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL
{
    public class PaginationEntity<T> where T: IEntity
    {
        public PaginationEntity(int recordCount)
        {
            RecordCount = recordCount;
        }

        public PaginationEntity()
        { }

        public List<T> List { get; set; }
        public int RecordCount { get; set; }

    }
}
