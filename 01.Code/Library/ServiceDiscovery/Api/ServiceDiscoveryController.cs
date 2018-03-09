using SOAFramework.Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace SOAFramework.Library
{
    [RoutePrefix("ServiceDiscovery")]
    public class ServiceDiscoveryController: ApiController
    {
        /// <summary>
        /// 获得服务信息
        /// </summary>
        /// <param name="serviceID"></param>
        /// <returns></returns>
        [Route("GetService/{id}")]
        public ServiceModel GetService(string id)
        {
            ServiceModel service = null;
            if (!String.IsNullOrEmpty(id))
            {
                HelpPageApiModel apiModel = Configuration.GetHelpPageApiModel(id);
                if (apiModel != null)
                {
                    service = apiModel.ToServiceModel();
                }
            }
            return service;
        }

        /// <summary>
        /// 获得类型信息
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        [Route("GetType/{typeName}")]
        public TypeModel GetType(string typeName)
        {
            TypeModel model = null;
            //if (!String.IsNullOrEmpty(typeName))
            //{
            //    ModelDescriptionGenerator modelDescriptionGenerator = Configuration.GetModelDescriptionGenerator();
            //    ModelDescription modelDescription;
            //    if (modelDescriptionGenerator.GeneratedModels.TryGetValue(typeName, out modelDescription))
            //    {
            //        model = modelDescription.ToTypeModel();
            //    }
            //}
            typeName = typeName.Replace("-", ".");
            model = typeName.ToTypeModel();
            return model;
        }

        /// <summary>
        /// 获得所有已解析的接口
        /// </summary>
        /// <returns></returns>
        [Route("GetServiceList")]
        public List<ServiceModel> GetServiceList()
        {
            List<ServiceModel> list = new List<ServiceModel>();
            var apis = GlobalConfiguration.Configuration.Services.GetApiExplorer().ApiDescriptions;
            foreach (var api in apis)
            {
                var model = api.ToServiceModel();
                list.Add(model);
            }
            return list;
        }
    }
}
