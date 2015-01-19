using DiscoverServiceWeb.SDK.Response;
using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscoverServiceWeb.SDK.Request
{
    public class DiscoverServiceByNameRequest : IRequest<DiscoverServiceByNameResponse>
    {
        public string GetApi()
        {
            return "SOAFramework.Service.Server.DefaultService.DiscoverServiceByName"; 
        }

        [ArgMapping("name")]
        public string Name { get; set; }
    }
}