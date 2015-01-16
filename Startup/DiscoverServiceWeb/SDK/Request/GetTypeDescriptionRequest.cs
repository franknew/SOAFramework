using DiscoverServiceWeb.SDK.Response;
using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscoverServiceWeb.SDK.Request
{
    public class GetTypeDescriptionRequest : IRequest<GetTypeDescriptionResponse>
    {
        [ArgMapping("fullTypeName")]
        public string FullTypeName { get; set; }
        public string Api
        {
            get { return "SOAFramework.Service.Server.DefaultService.GetTypeDescription"; }
        }
    }
}