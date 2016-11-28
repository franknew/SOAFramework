using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SOAFramework.Service.Core.Model
{
    [Serializable]
    [XmlRoot("configuration")]
    public class SOAConfiguration
    {
        [XmlElement("soaConfigGroup")]
        public SOAConfigGroup SOAConfig { get; set; }
    }

    [Serializable]
    public class SOAConfigGroup
    {
        [XmlElement("businessFileConfig", Type = typeof(ConfigSection))]
        public ConfigSection BusinessConfigSection { get; set; }

        [XmlElement("filterFileConfig", Type = typeof(ConfigSection))]
        public ConfigSection FilterConfigSection { get; set; }
    }

    public class ConfigSection
    {
        [XmlElement("config", Type = typeof(SOAConfigElement))]
        public SOAConfigElement[] Configs { get; set; }
    }

    [Serializable]
    public class SOAConfigElement
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("globalUse")]
        public bool GlobalUse { get; set; }
    }
}
