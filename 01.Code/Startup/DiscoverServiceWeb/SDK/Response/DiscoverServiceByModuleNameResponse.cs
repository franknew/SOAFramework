using DiscoverServiceWeb.Models;
using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscoverServiceWeb.SDK.Response
{
    public class DiscoverServiceByModuleNameResponse : BaseResponse
    {
        public List<ServiceInfo> Items { get; set; }
    }
}