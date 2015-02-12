using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscoverServiceWeb.Models
{
    public class ServiceInfo
    {
        public string InterfaceName { get; set; }

        public string Description { get; set; }

        private List<ServiceParameter> parameters = new List<ServiceParameter>();

        public List<ServiceParameter> Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        public string ReturnDesc { get; set; }

        public string Module { get; set; }

        private TypeInfo returnTypeInfo = new TypeInfo();

        public TypeInfo ReturnTypeInfo
        {
            get { return returnTypeInfo; }
            set { returnTypeInfo = value; }
        }
    }

    public class ServiceParameter
    {
        public string Name { get; set; }

        public int Index { get; set; }

        public string Description { get; set; }

        public TypeInfo TypeInfo { get; set; }
    }

    public class TypeInfo
    {
        public string TypeName { get; set; }

        public string FullTypeName { get; set; }

        public bool IsClass { get; set; }

        public string NameSpace { get; set; }
    }
}