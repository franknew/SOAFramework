using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.SDK.Core
{
    public abstract class BaseResponse
    {
        private string responseBody;

        internal void SetBody(string body)
        {
            this.responseBody = body;
        }

        public virtual string ResponseBody { get { return responseBody; } }
    }

    public abstract class DefaultResponse: BaseResponse
    {
        public bool IsError { get; set; }

        
        public virtual Exception Exception { get; set; }

        public virtual int Code { get; set; }

        public virtual string Message { get; set; }

        internal void SetValues(bool isError, int code, string message, Exception ex)
        {
            this.IsError = isError;
            this.Code = code;
            this.Exception = ex;
            this.Message = message;
        }
    }

    public sealed class BaseResponseShadow
    {
        public bool IsError { get; set; }

        public string ResponseBody { get; set; }

        public int Code { get; set; }

        public object Data { get; set; }

        public string Message { get; set; }

        public Exception Exception { get; set; }
    }
}
