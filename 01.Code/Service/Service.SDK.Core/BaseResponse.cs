using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.SDK.Core
{
    public abstract class BaseResponse
    {
        private bool isError = false;
        public bool IsError { get { return isError; } }

        private string responseBody;
        public string ResponseBody { get { return responseBody; } }

        private string errorMessage;
        public string ErrorMessage { get { return errorMessage; } }

        private string stackTrace;
        public string StackTrace { get { return stackTrace; } }

        internal void SetValues(bool isError,string errormsg, string stacktrace)
        {
            this.isError = isError;
            errorMessage = errormsg;
            this.stackTrace = stacktrace;
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

        public string ErrorMessage { get; set; }

        public string StackTrace { get; set; }
    }
}
