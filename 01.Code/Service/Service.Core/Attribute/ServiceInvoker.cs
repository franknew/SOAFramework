using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.Core
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ServiceInvoker : Attribute
    {
        public string InterfaceName { get; set; }

        public string Description { get; set; }

        public bool IsHiddenDiscovery { get; set; }

        private bool enabled = true;
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

    }

    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceLayer : Attribute
    {
        public bool IsHiddenDiscovery { get; set; }

        private bool enabled = true;
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }
    }
}
