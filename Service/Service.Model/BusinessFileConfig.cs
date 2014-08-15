using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.Model
{
    public class BusinessFileConfig : ConfigurationSection
    {
        [ConfigurationProperty("item", DefaultValue = "", IsRequired = true)]
        public List<BusinessFileConfigItem> Item 
        { 
            get
            {
                return this["item"] as List<BusinessFileConfigItem>;
            }
            set
            {
                this["item"] = value;
            }
        }
    }

    public class BusinessFileConfigItem : ConfigurationElement
    {

        [ConfigurationProperty("path", DefaultValue = "", IsRequired = true)]

        public string Path
        {
            get
            {
                return this["path"].ToString();
            }
            set
            {
                this["path"] = value;
            }
        }
    }
}
