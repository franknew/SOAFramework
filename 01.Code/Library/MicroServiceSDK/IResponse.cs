using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroService.Library.SDK
{
    public interface IResponse<T>
    {
        string JsonRpc { get; set; }
        T Result { get; set; }
        JsonRpcException Error { get; set; }
        object Id { get; set; }
    }
}
