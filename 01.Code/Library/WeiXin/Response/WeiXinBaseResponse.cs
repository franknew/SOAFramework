using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.WeiXin
{
    public class WeiXinBaseResponse
    {
        public string errcode { get; set; }
        public string errmsg { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
    }
}
