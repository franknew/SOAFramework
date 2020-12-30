﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL
{
    public class BaseQueryForm : IQueryForm
    {
        private string orderByColumn = "ID";
        private OrderBy orderby = OrderBy.ASC;

        public string OrderByColumn { get => orderByColumn; set => orderByColumn = value; }
        public OrderBy OrderBy { get => orderby; set => orderby = value; }
        public string ID { get; set; }

        public int? PageSize { get; set; }

        private int? currrentIndex = 1;
        public int? CurrentPageIndex 
        { 
            get
            {
                if ((!PageSize.HasValue || PageSize.Value <=0) && currrentIndex <=0)
                {
                    currrentIndex = 1;
                }
                return currrentIndex;
            }
            set
            {
                currrentIndex = value;
            }
        }

        public int? StartIndex
        {
            get
            {
                return PageSize * (CurrentPageIndex - 1);
            }
        }

        public int? EndIndex
        {
            get
            {
                return PageSize * CurrentPageIndex - 1;
            }
        }

        public int RecordCount { get; set; }

        public int PageCount
        {
            get
            {
                int pagecount = 0;
                if (PageSize.HasValue && PageSize > 0 && RecordCount > 0)
                {
                    pagecount = RecordCount / PageSize.Value;
                    if (RecordCount % PageSize > 0)
                    {
                        pagecount += 1;
                    }
                }
                return pagecount;
            }
        }

        public List<string> IDs { get; set; }
    }
}