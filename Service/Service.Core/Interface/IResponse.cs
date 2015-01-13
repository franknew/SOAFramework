using SOAFramework.Service.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.Interface
{
    [ServiceLayer(Enabled = false)]
    public interface IResponse
    {
        bool IsError { get; set; }
        string ErrorMessage { get; set; }
        string StackTrace { get; set; }
        string Body { get; set; }

    }
}
