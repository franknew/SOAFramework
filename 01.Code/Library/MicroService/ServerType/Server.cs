using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AustinHarris.JsonRpc;
using System.Reflection;

namespace MicroService.Library
{
    public class Server : IServer
    {
        public void Bind(string sessionName, AppDomain domain, List<Assembly> assList)
        {
            try
            {
                ServiceBinder.BindService(sessionName, domain: domain, assList: assList);
                FilterBinder.BindFilter(sessionName, domain: domain, assList: assList);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
