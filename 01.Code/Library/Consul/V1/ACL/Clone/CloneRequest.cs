using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.SDK.V1.ACL.Clone
{
    [Put]
    public class CloneRequest : BaseRequest<ACLResponse>
    {
        public override string GetApi()
        {
            return "/v1/acl/clone/{ID}";
        }

        public string ID { get; set; }
    }
}
