using AustinHarris.JsonRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace MicroService.Library.Module
{
    [JsonRpcClass]
    public class ServiceContoller
    {
        [JsonRpcMethod]
        public bool Start(string name)
        {
            var service = GetService(name);
            service.Start();
            service.WaitForStatus(ServiceControllerStatus.Running);
            return true;
        }

        [JsonRpcMethod]
        public bool Stop(string name)
        {
            var service = GetService(name);
            service.Stop();
            service.WaitForStatus(ServiceControllerStatus.Stopped);
            return true;
        }

        [JsonRpcMethod]
        public bool Pause(string name)
        {
            var service = GetService(name);
            service.Pause();

            service.WaitForStatus(ServiceControllerStatus.Paused);
            return true;
        }

        [JsonRpcMethod]
        public bool Continue(string name)
        {
            var service = GetService(name);
            service.Stop();
            service.WaitForStatus(ServiceControllerStatus.Running);
            return true;
        }

        [JsonRpcMethod]
        public ServiceEntity GetServiceInfo(string name)
        {
            var service = GetService(name);
            return ServiceToEntity(service);
        }

        [JsonRpcMethod]
        public List<ServiceEntity> GetServices(string nameFilter, string status)
        {
            List<ServiceEntity> list = new List<ServiceEntity>();
            var services = ServiceController.GetServices();
            ServiceControllerStatus parseStatus = ServiceControllerStatus.Running;
            Enum.TryParse<ServiceControllerStatus>(status, out parseStatus);
            foreach (var service in services)
            {
                if (string.IsNullOrEmpty(nameFilter) || service.ServiceName.Contains(nameFilter) || service.DisplayName.Contains(nameFilter))
                {
                    list.Add(ServiceToEntity(service));
                }
            }
            return list;
        }

        private ServiceController GetService(string name)
        {
            var services = ServiceController.GetServices();
            var service = services.FirstOrDefault(t => t.ServiceName.ToLower().Equals(name.ToLower()));
            if (service == null) throw new Exception(string.Format("服务名称:{0} 在本机不存在", name));
            return service;
        }

        private ServiceEntity ServiceToEntity(ServiceController service)
        {
            if (service == null) return null;
            ServiceEntity entity = new ServiceEntity
            {
                ServiceName = service.ServiceName,
                DisplayName = service.DisplayName,
                Status = service.Status,
                Type = service.ServiceType,
                MachineName = service.MachineName,
            };
            return entity;
        }
    }
}
