using DiscoverServiceWeb.SDK.Response;
using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscoverServiceWeb.SDK.Request
{
    public class GetModuleNamesRequest : IRequest<GetModuleNamesResponse>
    {
        public string Api
        {
            get { return "SOAFramework.Service.Server.DefaultService.GetServiceModuleNames"; }
        }
    }
}