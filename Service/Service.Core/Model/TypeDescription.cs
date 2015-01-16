using SOAFramework.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.Core.Model
{
    public class TypeDescription
    {
        public TypeInfo TypeInfo { get; set; }

        public List<PropertyDescription> Properties { get; set; }
    }
}
