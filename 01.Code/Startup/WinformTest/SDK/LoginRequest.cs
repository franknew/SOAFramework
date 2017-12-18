using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinformTest.SDK
{
    public class LoginRequest : IRequest<BaseResponse>
    {
        public string GetApi()
        {
            return "Account/Login";
        }

        public string userName { get; set; }

        public string passPwd { get; set; }
    }
}
