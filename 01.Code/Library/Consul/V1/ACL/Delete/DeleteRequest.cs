using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.SDK.V1.ACL.Delete
{
    [Put]
    public class DeleteRequest : BaseRequest<BaseResponse>
    {
        public override string GetApi()
        {
            return "/v1/acl/destroy/{ID}";
        }

        public string ID { get; set; }
    }
}
