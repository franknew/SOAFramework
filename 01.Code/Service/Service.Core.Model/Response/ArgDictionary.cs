using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.Core.Model
{
    [DataContract]
    public class PostArgItem
    {
        [DataMember(EmitDefaultValue = false)]
        public string Key { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public object Value { get; set; }

    }
}
