using SOAFramework.Service.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.Core
{
    public interface IDispatcher
    {
        Stream Execute(string typeName, string functionName, Dictionary<string, string> args, List<BaseFilter> filterList,
            bool enableConsoleMonitor);

        void StartRegisterTask(string dispatchServerUrl);
    }
}
