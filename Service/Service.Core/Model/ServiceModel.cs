using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.Model
{
    [DataContract]
    public class ServiceModel
    {
        [DataMember(EmitDefaultValue = false)]
        public ServiceInfo ServiceInfo { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public MethodInfo MethodInfo { get; set; }
    }
}
