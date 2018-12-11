using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.SDK.Domain
{
    public class Check
    {
        public string Node { get; set; }
        public string CheckID { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
        public string ServiceID { get; set; }
        public CheckDefinition Definition { get; set; }
        public string Output { get; set; }
        public string ServiceName { get; set; }
        public List<string> ServiceTags { get; set; }
        public string ID { get; set; }
    }
}
