using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.Server
{
    public static class WcfHostExtension
    {
        public static bool Ping(this ServiceHost host)
        {
            bool valid = true;
            foreach (var p in host.Description.Endpoints)
            {
                try
                {
                    HttpHelper.Get(p.ListenUri.AbsoluteUri.TrimEnd('/') + "/Ping", 3000);
                }
                catch
                {
                    valid = false;
                    return valid;
                }
            }
            return valid;
        }
    }
}
