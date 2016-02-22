using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.WeiXin
{
    public class Department_DeleteRequest : WeiXinBaseRequest<WeiXinBaseResponse>
    {
        public override string GetApi()
        {
            return "department/delete";
        }

        public CommonIDQueryString QueryString { get; set; }
    }
}
