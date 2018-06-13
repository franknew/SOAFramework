using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Core;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using Aliyun.Acs.Core.Exceptions;
using Newtonsoft.Json;

namespace SOAFramework.Library
{
    /// <summary>
    /// 阿里云短信发送客户端
    /// </summary>
    public class AliSmsClient : ISmsClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        public AliSmsClient(string appKey, string appSecret)
        {
            if (string.IsNullOrEmpty(appKey)) { throw new Exception("没有配置SmsAppKey"); }
            if (string.IsNullOrEmpty(appSecret)) { throw new Exception("没有配置SmsAppSecret"); }
            this.appKey = appKey;
            this.appSecret = appSecret;
        }

        /// <summary>
        /// 
        /// </summary>
        public AliSmsClient()
        {
            string key = ConfigurationManager.AppSettings["SmsAppKey"];
            string secret = ConfigurationManager.AppSettings["SmsAppSecret"];
            if (string.IsNullOrEmpty(key)) { throw new Exception("没有配置SmsAppKey"); }
            if (string.IsNullOrEmpty(secret)) { throw new Exception("没有配置SmsAppSecret"); }
            this.appKey = key;
            this.appSecret = secret;
        }

        //产品名称:云通信短信API产品,开发者无需替换
        const String product = "Dysmsapi";
        //产品域名,开发者无需替换
        const String domain = "dysmsapi.aliyuncs.com";

        private string appKey;
        private string appSecret;

        public string AppKey => appKey;

        public string AppSecret => appSecret;

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="signName">签名名称，此处应在云平台维护</param>
        /// <param name="templateCode">模板编号，应在云平台维护</param>
        /// <param name="templateParam">模板中的参数</param>
        /// <param name="phoneNumbers">电话号码，数量不能大于1000</param>
        /// <returns></returns>
        public SmsResult Send(string signName, string templateCode, object templateParam, string[] phoneNumbers)
        {
            SmsResult result = new SmsResult();
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", appKey, appSecret);
            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();
            SendSmsResponse response = null;
            if (phoneNumbers == null) { throw new Exception("必须设置手机号码"); }
            if (phoneNumbers.Length > 1000) { throw new Exception("不能超过1000个手机号码"); }
            try
            {
                string numbers = String.Join(",", phoneNumbers);
                //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为1000个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式
                request.PhoneNumbers = numbers;
                //必填:短信签名-可在短信控制台中找到
                request.SignName = signName;
                //必填:短信模板-可在短信控制台中找到
                request.TemplateCode = templateCode;
                string json = JsonConvert.SerializeObject(templateParam);
                //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
                request.TemplateParam = json;
                request.AcceptFormat = Aliyun.Acs.Core.Http.FormatType.JSON;
                //可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
                //request.OutId = "yourOutId";
                //请求失败这里会抛ClientException异常
                response = acsClient.GetAcsResponse(request);

            }
            catch (Exception ex)
            {
                result.IsOK = false;
                result.Message = ex.Message;
                return result;
                //Console.WriteLine(e1.ErrorCode);
            }
            result.IsOK = response.Code.Equals("OK");
            result.Message = response.Message;
            result.BizID = response.BizId;
            result.RequestID = response.RequestId;
            return result;
        }
    }
}
