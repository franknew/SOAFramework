using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.SDK.V1.ACL.Info
{
    [Get]
    public class InfoRequest : BaseRequest<InfoResponse>
    {
        public override string GetApi()
        {
            return "/v1/acl/info/{ID}";
        }

        public string ID { get; set; }
    }
}
