using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.SDK.V1.Agent.Services
{
    [Put]
    public class RegisterRequest : BaseRequest<RegisterResponse>
    {
        public override string GetApi()
        {
            return "/v1/agent/service/register";
        }

        [PostData]
        public RegisterPayload Payload { get; set; }
    }
}
