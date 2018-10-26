using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.SDK.V1.Agent.Services
{
    public class ServicesRequest : BaseRequest<ServicesResponse>
    {
        public override string GetApi()
        {
            return "/v1/agent/services";
        }
    }
}
