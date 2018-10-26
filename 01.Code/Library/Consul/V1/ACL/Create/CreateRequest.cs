using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.SDK.V1.ACL.Create
{
    [Put]
    public class CreateRequest : BaseRequest<ACLResponse>
    {
        public override string GetApi()
        {
            return "/v1/acl/create";
        }

        public string ID { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Rules { get; set; }
    }
}
