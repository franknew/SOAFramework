using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.SDK.Domain
{
    public class Replication
    {
        public bool? Enabled { get; set; }
        public bool? Running { get; set; }
        public string SourceDatacenter { get; set; }
        public int? ReplicatedIndex { get; set; }
        public DateTime? LastSuccess { get; set; }
        public DateTime? LastError { get; set; }
    }
}
