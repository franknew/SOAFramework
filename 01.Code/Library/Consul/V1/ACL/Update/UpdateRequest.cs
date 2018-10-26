using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.SDK.V1.ACL.Update
{
    [Put]
    public class UpdateRequest : BaseRequest<ACLResponse>
    {
        public override string GetApi()
        {
            return "/v1/acl/update";
        }
    }
}
