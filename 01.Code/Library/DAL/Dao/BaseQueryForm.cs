using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL
{
    public class BaseQueryForm
    {
        public string ID { get; set; }

        public int PageSize { get; set; }

        public int CurrentIndex { get; set; }

        public int StartIndex
        {
            get
            {
                return PageSize * (CurrentIndex - 1);
            }
        }

        public int EndIndex
        {
            get
            {
                return PageSize * CurrentIndex - 1;
            }
        }

        public int RecordCount { get; set; }

        public int PageCount
        {
            get
            {
                int pagecount = 0;
                if (PageSize > 0)
                {
                    pagecount = RecordCount / PageSize;
                    if (RecordCount % PageSize > 0)
                    {
                        pagecount += 1;
                    }
                }
                return pagecount;
            }
        }
    }
}
