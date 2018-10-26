using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.SDK.V1.ACL.Replication
{
    public class ReplicationResponse: BaseResponse
    {
        [SDKResult]
        public SOAFramework.Library.SDK.Domain.Replication Result { get; set; }
    }
}
