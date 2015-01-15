using DiscoverServiceWeb.Models;
using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscoverServiceWeb.SDK.Response
{
    public class DiscoverServiceByNameResponse : BaseResponse
    {
        public ServiceDiscovery ServiceInfo { get; set; }
    }
}