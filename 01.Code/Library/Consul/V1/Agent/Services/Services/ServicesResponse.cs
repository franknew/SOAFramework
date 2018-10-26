using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.SDK.V1.Agent.Services
{
    public class ServicesResponse: BaseResponse
    {
        [SDKResult]
        public ServiceDomain Services { get; set; }
    }
}
