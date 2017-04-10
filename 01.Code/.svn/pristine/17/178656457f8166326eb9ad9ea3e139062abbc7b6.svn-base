using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.SDK.Core
{
    public interface IRequest<T> where T : BaseResponse
    {
        string GetApi();
    }

    public abstract class BaseRequest<T> : IRequest<T> where T : BaseResponse
    {
        public abstract string GetApi();

        public RequestBody Body { get; set; }
    }

    public class RequestBody
    {
        public string URL { get; set; }

        public string PostData { get; set; }
    }
}
