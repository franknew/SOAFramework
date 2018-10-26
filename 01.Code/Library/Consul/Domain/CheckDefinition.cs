using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.SDK.Domain
{
    public class CheckDefinition
    {
        public string DeregisterCriticalServiceAfter { get; set; }
        public List<string> Args { get; set; }
        public string HTTP { get; set; }
        public string Interval { get; set; }
        public string TTL { get; set; }
        public string TCP { get; set; }
        public string Timeout { get; set; }
    }
}
