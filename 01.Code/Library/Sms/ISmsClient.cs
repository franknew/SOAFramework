using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public interface ISmsClient
    {
        /// <summary>
        /// 
        /// </summary>
        string AppKey { get; }
        /// <summary>
        /// 
        /// </summary>
        string AppSecret { get; }
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="signName">签名名称，云平台维护</param>
        /// <param name="templateCode">模板编号，云平台维护</param>
        /// <param name="templateParams">模板参数</param>
        /// <param name="phoneNumbers">电话号码</param>
        /// <returns></returns>
        SmsResult Send(string signName, string templateCode, object templateParams, string[] phoneNumbers);
    }

    public class SmsClientFactory
    {
        public static ISmsClient Create(SmsTypeEnum type, string appKey = null, string appSecret = null)
        {
            ISmsClient client = null;
            switch (type)
            {
                case SmsTypeEnum.Aliyun:
                    if (string.IsNullOrEmpty(appKey) && string.IsNullOrEmpty(appSecret)) { client = new AliSmsClient(); }
                    else { client = new AliSmsClient(appKey, appSecret); }
                    break;
            }
            return client;
        }
    }
}
