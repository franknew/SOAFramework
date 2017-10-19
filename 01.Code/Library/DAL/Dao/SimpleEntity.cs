using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL
{
    public class SimpleEntity : BaseEntity
    {

        public string Name { get; set; }

        public string Creator { get; set; }

        public DateTime? CreateTime { get; set; }

        public string LastUpdator { get; set; }

        public DateTime? LastUpdateTime { get; set; }
    }
}
