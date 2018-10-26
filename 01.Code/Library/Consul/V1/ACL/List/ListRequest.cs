using SOAFramework.Library.SDK.V1.ACL.Info;
using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.SDK.V1.ACL.List
{
    [Get]
    public class ListRequest : BaseRequest<InfoResponse>
    {
        public override string GetApi()
        {
            return "/v1/acl/list";
        }
    }
}
