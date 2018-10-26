using SOAFramework.Library.SDK.Domain;
using SOAFramework.Library.SDK.V1.Agent.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.SDK.V1.Catalog.Register
{
    public class RegisterPayload
    {
        public string Datacenter { get; set; }
        public string ID { get; set; }
        public string Node { get; set; }
        public string Address { get; set; }
        public TargetAddress TaggedAddresses { get; set; }
        public ServiceDomain Service { get; set; }
        public Check Check { get; set; }
        public bool? SkipNodeUpdate { get; set; }
    }
}
