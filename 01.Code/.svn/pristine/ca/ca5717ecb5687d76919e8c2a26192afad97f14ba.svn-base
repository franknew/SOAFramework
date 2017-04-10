using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AustinHarris.JsonRpc;

namespace MicroService.Library
{
    public class Server : IServer
    {
        public void Bind(string sessionName, AppDomain domain)
        {
            ServiceBinder.BindService(sessionName, domain);
            FilterBinder.BindFilter(sessionName, domain);
        }
    }
}
