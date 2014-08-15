using SOAFramework.Service.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.Server
{
    public class Test
    {
        [ServiceInvoker]
        public string TestMethod(string str, string str1)
        {
            return "hell world! " + str;
        }
    }


}
