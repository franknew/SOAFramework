using DiscoverServiceWeb.Models;
using DiscoverServiceWeb.SDK.Request;
using DiscoverServiceWeb.SDK.Response;
using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DiscoverServiceWeb.Controllers
{
    public class ServiceDiscoveryController : Controller
    {
        //
        // GET: /ServiceDiscovery/
        
        public ActionResult Index()
        {
            ServiceDiscoveryRequest request = new ServiceDiscoveryRequest();
            ServiceDiscoveryResponse response = SDKFactory.Client.Execute(request);
            this.ViewBag.Data = response.Items;
            return View("Index");
        }

        public ActionResult ServiceDetail(string id)
        {
            string interfaceName = id.Replace("-", ".");
            DiscoverServiceByNameRequest request = new DiscoverServiceByNameRequest();
            request.Name = interfaceName;
            DiscoverServiceByNameResponse response = SDKFactory.Client.Execute(request);
            if (response.ServiceInfo != null)
            {
                if (response.ServiceInfo.Parameters == null)
                {
                    response.ServiceInfo.Parameters = new List<ServiceParameter>();
                }
                this.ViewBag.Data = response.ServiceInfo;
            }
            else
            {
                this.ViewBag.Data = new ServiceDiscovery { Parameters = new List<ServiceParameter>() };
            }
            return View();
        }

    }
}
