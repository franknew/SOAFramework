using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.SDK.V1.Agent.Services.Maintenance
{
    [Put]
    public class MaintenanceRequest : BaseRequest<BaseResponse>
    {
        public override string GetApi()
        {
            return "/v1/agent/service/maintenance/{ServiceID}";
        }

        public string ServiceID { get; set; }
    }
}
