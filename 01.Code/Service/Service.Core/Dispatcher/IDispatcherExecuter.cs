using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.Core
{
    [ServiceLayer(IsHiddenDiscovery = true)]
    public interface IDispatcherExecuter
    {
        string Execute(string endPoint, string typeName, string functionName, Dictionary<string, string> args);
    }

    public class DispatcherExecuterFactory
    {
        public static IDispatcherExecuter CreateExecuter(DispatcherExecuterType type)
        {
            IDispatcherExecuter exec = null;
            switch (type)
            {
                case DispatcherExecuterType.Http:
                    exec = new HttpDispatcherExecuter();
                    break;
            }
            return exec;
        }
    }

    public enum DispatcherExecuterType
    {
        Http,
        Remote,
        Wcf,
    }
}
