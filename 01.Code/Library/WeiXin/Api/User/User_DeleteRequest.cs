using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.WeiXin
{
    public class User_DeleteRequest : WeiXinBaseRequest<WeiXinBaseResponse>
    {
        public override string GetApi()
        {
            return "user/delete";
        }

        public User_DeleteQueryString QueryString { get; set; }
    }
}
