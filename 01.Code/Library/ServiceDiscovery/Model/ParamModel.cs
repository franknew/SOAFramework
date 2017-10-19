using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SOAFramework.Library
{
    public class ParamModel
    {
        [XmlAttribute(attributeName:"name")]
        public string Name { get; set; }
        [XmlText]
        public string Description { get; set; }
    }
}
