using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.WeiXin
{
    public class GetTokenResponse
    {
        public string access_token { get; set; }

        public int expires_in { get; set; }
    }
}
