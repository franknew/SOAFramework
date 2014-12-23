using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Client.SDK.Core
{
    public interface IRequest<T> where T : BaseResponse
    {
        string GetApi();
    }
}
