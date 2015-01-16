using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SOAFramework.Service.SDK.Core;
using DiscoverServiceWeb.Models;

namespace DiscoverServiceWeb.SDK.Response
{
    public class ServiceDiscoveryResponse : BaseResponse
    {
        public List<ServiceInfo> Items { get; set; }
    }
}