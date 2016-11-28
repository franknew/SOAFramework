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

    }
}
