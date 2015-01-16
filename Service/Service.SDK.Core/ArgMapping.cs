using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.SDK.Core
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ArgMapping : Attribute
    {
        public ArgMapping(string mapping)
        {
            Mapping = mapping;
        }

        public string Mapping { get; set; }
    }
}
