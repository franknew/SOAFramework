using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.SDK.V1.Agent.Services.Deregister
{
    [Put]
    public class DeregisterRequest : BaseRequest<BaseResponse>
    {
        public override string GetApi()
        {
            return "/v1/agent/service/deregister/{ServiceID}";
        }

        public string ServiceID { get; set; }
    }
}
