using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.SDK.Core
{
    public abstract class BaseResponse
    {
        public bool IsError { get; set; }

        private string responseBody;
        public virtual string ResponseBody { get { return responseBody; } }
        
        public virtual Exception Exception { get; set; }

        public virtual int Code { get; set; }

        internal void SetValues(bool isError, int code, Exception ex)
        {
            this.IsError = isError;
            this.Code = code;
            this.Exception = ex;
        }

        internal void SetBody(string body)
        {
            this.responseBody = body;
        }
    }

    public sealed class BaseResponseShadow
    {
        public bool IsError { get; set; }

        public string ResponseBody { get; set; }

        public int Code { get; set; }

        public object Data { get; set; }

        public Exception Exception { get; set; }
    }
}
