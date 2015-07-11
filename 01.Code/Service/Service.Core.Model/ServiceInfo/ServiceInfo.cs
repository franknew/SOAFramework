using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.Core.Model
{
    [DataContract]
    public class ServiceInfo
    {

        [DataMember(EmitDefaultValue = false)]
        public string InterfaceName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Description { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<ServiceParameter> Parameters { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ReturnDesc { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Module { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public TypeInfo ReturnTypeInfo { get; set; }

        public bool IsHidden { get; set; }

        public Assembly Assembly { get; set; }
    }

    [DataContract]
    public class ServiceParameter
    {
        [DataMember(EmitDefaultValue = false)]
        public string Name { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int Index { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Description { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public TypeInfo TypeInfo { get; set; }
    }

    [DataContract]
    public class TypeInfo
    {
        [DataMember(EmitDefaultValue = false)]
        public string TypeName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string FullTypeName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsClass { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string NameSpace { get; set; }
        public string ElementFullTypeName { get; set; }
    }
}
