
using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.SDK.V1.Health.ListNode
{
    public class ListNodeRequest : BaseRequest<ListNodeResponse>
    {
        public override string GetApi()
        {
            return "v1/health/node/{NodeID}";
        }

        public string NodeID { get; set; }
    }
}
