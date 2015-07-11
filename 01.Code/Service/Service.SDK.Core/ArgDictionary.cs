using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.SDK.Core
{
    [DataContract]
    public class PostArgItem 
    {
        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public object Value { get; set; }

    }

    [DataContract]
    public class PostData
    {
        [DataMember]
        public string Data { get; set; }
    }
}
