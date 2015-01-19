using DiscoverServiceWeb.SDK.Response;
using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscoverServiceWeb.SDK.Request
{
    public class DiscoverServiceByModuleNameRequest : IRequest<DiscoverServiceByModuleNameResponse>
    {
        public string GetApi()
        {
            return "SOAFramework.Service.Server.DefaultService.DiscoverServiceByModule"; 
        }

        [ArgMapping("moduleName")]
        public string ModuleName { get; set; }
    }
}