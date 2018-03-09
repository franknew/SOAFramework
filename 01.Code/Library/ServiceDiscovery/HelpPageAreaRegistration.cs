using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace SOAFramework.Library
{
    public class HelpPageAreaRegistration: AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ServiceDiscovery";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            HelpPageConfig.Register(GlobalConfiguration.Configuration);
        }
    }
}
