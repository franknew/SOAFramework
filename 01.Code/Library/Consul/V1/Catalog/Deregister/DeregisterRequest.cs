using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.SDK.V1.Catalog.Deregister
{
    [Put]
    public class DeregisterRequest : BaseRequest<BaseResponse>
    {
        public override string GetApi()
        {
            return "/v1/catalog/deregister";
        }

        [PostData]
        public DeregisterPayload Payload { get; set; }
    }
}
