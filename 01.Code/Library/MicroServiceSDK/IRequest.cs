using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroService.Library.SDK
{
    public interface IRequest<T> where T: BaseResponse
    {
        string GetApi();
        string 

    }
}
