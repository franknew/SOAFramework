using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscoverServiceWeb.Models
{
    public class ServiceDiscovery
    {
        public string InterfaceName { get; set; }

        public string Description { get; set; }

        public List<ServiceParameter> Parameters { get; set; }

        public string ReturnDesc { get; set; }
    }

    public class ServiceParameter
    {
        public string Name { get; set; }

        public string TypeName { get; set; }

        public int Index { get; set; }

        public string Description { get; set; }
    }
}