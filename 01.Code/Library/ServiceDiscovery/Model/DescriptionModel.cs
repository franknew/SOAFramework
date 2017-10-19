using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SOAFramework.Library
{
    [XmlRoot("doc")]
    public class DescriptionModel
    {
        [XmlArray("members")]
        [XmlArrayItem("member")]
        public List<MemberDescription> Members { get; set; }
    }
}
