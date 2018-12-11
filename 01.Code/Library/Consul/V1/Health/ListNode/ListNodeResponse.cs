using SOAFramework.Library.SDK.Domain;
using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.SDK.V1.Health.ListNode
{
    public class ListNodeResponse : BaseResponse
    {
        [SDKResult]
        public List<Check> Checks { get; set; }
    }
}
