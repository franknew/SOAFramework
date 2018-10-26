using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.SDK.V1.ACL.Replication
{
    [Get]
    public class ReplicationRequest : BaseRequest<ReplicationResponse>
    {
        public override string GetApi()
        {
            return "/v1/acl/replication";
        }
    }
}
