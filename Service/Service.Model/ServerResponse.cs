using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.Model
{
    [DataContract]
    public class ServerResponse
    {
        [DataMember(EmitDefaultValue = false)]
        public bool? IsError { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ErrorMessage { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string StackTrace { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public object Data { get; set; }
    }
}
