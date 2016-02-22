using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.WeiXin
{
    public class Department_ListResponse: WeiXinBaseResponse
    {
        public List<Department> department { get; set; }
    }
}
