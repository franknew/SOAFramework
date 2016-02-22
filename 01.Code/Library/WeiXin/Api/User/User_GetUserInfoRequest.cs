using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.WeiXin
{
    public class User_GetUserInfoRequest : WeiXinBaseRequest<User_GetUserInfoResponse>
    {
        public override string GetApi()
        {
            return "user/getuserinfo";
        }

        public User_GetUserInfoQueryString QueryString { get; set; }
    }
}
