using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL
{
    public class SimpleQueryForm : BaseQueryForm
    {

        public string Name { get; set; }

        public string Creator { get; set; }

        public DateTime? CreateTime_Start { get; set; }

        public DateTime? CreateTime_End { get; set; }

        public string LastUpdator { get; set; }

        public DateTime? LastUpdateTime_Start { get; set; }

        public DateTime? LastUpdateTime_End { get; set; }
    }
}
