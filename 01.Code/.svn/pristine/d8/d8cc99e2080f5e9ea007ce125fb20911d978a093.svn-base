using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroService.Library
{
    public interface IServer
    {
        void Bind(string sessionName, AppDomain domain);
    }

    public class ServerTypeFactory
    {
        public static IServer Create(ServerType type)
        {
            IServer server = null;
            switch (type)
            {
                case ServerType.Server:
                    server = new Server();
                    break;
                case ServerType.Timing:
                    server = new Timer();
                    break;
            }
            return server;
        }
    }
}
