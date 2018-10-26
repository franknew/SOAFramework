using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.SDK.V1.Catalog.Deregister
{
    public class DeregisterPayload
    {
        public string Datacenter { get; set; }
        public string Node { get; set; }
        public string CheckID { get; set; }
        public string ServiceID { get; set; }
    }
}
