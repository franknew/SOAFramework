using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SOAFramework.Library
{
    public class MemberDescription
    {
        [XmlAttribute(attributeName: "name")]
        public string Name { get; set; }
        [XmlElement(elementName: "summary")]
        public string Summary { get; set; }
        [XmlElement(elementName: "param")]
        public List<ParamModel> Params { get; set; }
        [XmlElement(elementName: "returns")]
        public string Returns { get; set; }
    }
}
