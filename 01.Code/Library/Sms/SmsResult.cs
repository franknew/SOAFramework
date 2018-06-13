using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public class SmsResult
    {
        /// <summary>
        /// 成功或者异常信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 是否发送成功
        /// </summary>
        public bool IsOK { get; set; }

        /// <summary>
        /// 本次请求ID，由云平台返回
        /// </summary>
        public string RequestID { get; set; }

        /// <summary>
        /// 业务ID，由云平台返回
        /// </summary>
        public string BizID { get; set; }
    }
}
