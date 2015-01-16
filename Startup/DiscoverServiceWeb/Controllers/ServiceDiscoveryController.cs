using DiscoverServiceWeb.Models;
using DiscoverServiceWeb.SDK.Request;
using DiscoverServiceWeb.SDK.Response;
using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DiscoverServiceWeb.Controllers
{
    public class ServiceDiscoveryController : Controller
    {
        //
        // GET: /ServiceDiscovery/

        public ActionResult Index(string id)
        {
            string moduleName = "Default";
            if (!string.IsNullOrEmpty(id))
            {
                moduleName = id;
            }
            GetModuleNamesRequest gmnRequest = new GetModuleNamesRequest();
            GetModuleNamesResponse gmnResponse = SDKFactory.Client.Execute(gmnRequest);

            DiscoverServiceByModuleNameRequest moduleRequest = new DiscoverServiceByModuleNameRequest();
            moduleRequest.ModuleName = moduleName;
            DiscoverServiceByModuleNameResponse moduleResponse = SDKFactory.Client.Execute(moduleRequest);

            if (moduleResponse.Items == null)
            {
                moduleResponse.Items = new List<ServiceInfo>();
            }
            if (gmnResponse == null)
            {
                gmnResponse.Items = new List<string>();
            }
            this.ViewBag.Modules = gmnResponse.Items;
            this.ViewBag.Services = moduleResponse.Items;
            this.ViewBag.CurrentModule = moduleName;
            return View();
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
                this.ViewBag.Data = new ServiceInfo { Parameters = new List<ServiceParameter>() };
            }
            return View();
        }

        public ActionResult TypeDescription(string id)
        {
            string typeName = id.Replace("-", ".");
            GetTypeDescriptionRequest request = new GetTypeDescriptionRequest();
            request.FullTypeName = typeName;
            GetTypeDescriptionResponse response = SDKFactory.Client.Execute(request);
            if (response.Item == null)
            {
                response.Item = new TypeDescription();
            }
            this.ViewBag.Data = response.Item;
            return View();
        }

        public JsonResult GetTypeDescription(string id)
        {
            string typeName = id.Replace("-", ".");
            GetTypeDescriptionRequest request = new GetTypeDescriptionRequest();
            request.FullTypeName = typeName;
            GetTypeDescriptionResponse response = SDKFactory.Client.Execute(request);
            if (response.Item == null)
            {
                response.Item = new TypeDescription();
            }
            return Json(response.Item, JsonRequestBehavior.AllowGet);
        }

        public FileResult GenerateCodeFile(string id)
        {
            string fullTypeName = id.Replace("-", ".");
            StringBuilder builder = new StringBuilder();
            List<string> emittedType = new List<string>();
            builder.Append("using System;\r\n");
            builder.Append("using System.Text;\r\n");
            builder.Append("using System.Linq;\r\n");
            builder.Append("using System.Collections.Generic;\r\n");
            GenerateType(fullTypeName, builder, emittedType);
            string code = builder.ToString();
            byte[] bytCode = Encoding.UTF8.GetBytes(code);
            return File(bytCode, "application/octet-stream", fullTypeName + ".cs");
        }

        public void GenerateType(string fullTypeName, StringBuilder builder, List<string> emittedType)
        {
            if (emittedType.Contains(fullTypeName))
            {
                return;
            }
            List<string> list = new List<string>();
            GetTypeDescriptionRequest request = new GetTypeDescriptionRequest();
            request.FullTypeName = fullTypeName;
            GetTypeDescriptionResponse response = SDKFactory.Client.Execute(request);
            if (response.Item != null)
            {
                TypeDescription type = response.Item;
                builder.Append("namespace ").Append(type.TypeInfo.NameSpace).Append("\r\n{\r\n");
                builder.Append("\tpublic class ").Append(type.TypeInfo.TypeName).Append("\r\n\t{\r\n");
                foreach (var p in response.Item.Properties)
                {
                    if (p.PropertyTypeInfo.IsClass)
                    {
                        list.Add(p.PropertyTypeInfo.FullTypeName);
                    }
                    builder.Append("\t\tpublic ").Append(p.PropertyTypeInfo.TypeName).Append(" ").Append(p.PropertyName).Append(" { get; set; }\r\n");
                }
                builder.Append("\t}\r\n}\r\n");
                emittedType.Add(fullTypeName);

                foreach (var t in list)
                {
                    GenerateType(t, builder, emittedType);
                }
            }
        }
    }
}
