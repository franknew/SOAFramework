using SOAFramework.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.Core.Model
{
    [DataContract]
    public class TypeDescription
    {
        [DataMember(EmitDefaultValue = false)]
        public TypeInfo TypeInfo { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<PropertyDescription> Properties { get; set; }
    }
}
