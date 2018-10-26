using SOAFramework.Library.SDK.Domain;
using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.SDK.V1.ACL.Info
{
    public class InfoResponse: BaseResponse
    {
        [SDKResult]
        public List<ACLInfo> infos { get; set; }
    }
}
