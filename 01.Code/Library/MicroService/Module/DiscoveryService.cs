using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AustinHarris.JsonRpc;

namespace MicroService.Library
{
    [JsonRpcClass]
    public class DiscoveryService
    {
        [JsonRpcMethod]
        public List<NodeServerDataModel> GetPackages()
        {
            return MasterServer.GetPackages();
        }

        [JsonRpcMethod]
        public List<NodeServerDataModel> GetTimings()
        {
            return MasterServer.GetTimings();
        }

        [JsonRpcMethod]
        public bool RestartService(string service)
        {
            MasterServer server = new MasterServer();
            server.RestartNode(service);
            return true;
        }

        [JsonRpcMethod]
        public bool RestartAllService()
        {
            MasterServer server = new MasterServer();
            server.StartAllNode();
            return true;
        }
    }
}
