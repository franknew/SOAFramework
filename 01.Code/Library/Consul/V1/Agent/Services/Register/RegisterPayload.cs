using SOAFramework.Library.SDK.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.SDK.V1.Agent.Services
{
    public class RegisterPayload
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public List<string> Tags { get; set; }
        public string Address { get; set; }
        public int? Port { get; set; }
        public Meta Meta { get; set; }
        public bool? EnableTagOverride { get; set; }

    }
}
