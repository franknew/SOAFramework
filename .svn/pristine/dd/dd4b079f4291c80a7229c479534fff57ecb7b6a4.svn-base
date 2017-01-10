using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace SOAFramework.Library.WeiXin
{
    public class SignIn
    {
        private string corpID;
        private string corpSecret;

        public string CorpID
        {
            get
            {
                return corpID;
            }

            set
            {
                corpID = value;
            }
        }

        public string CorpSecret
        {
            get
            {
                return corpSecret;
            }

            set
            {
                corpSecret = value;
            }
        }

        public SignIn(string corpid, string corpsecret)
        {
            this.corpID = corpid;
            this.corpSecret = corpsecret;
        }

        public SignIn()
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["corpid"])) corpID = ConfigurationManager.AppSettings["corpid"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["corpsecret"])) corpSecret = ConfigurationManager.AppSettings["corpsecret"];
        }

        public string Do()
        {
            if (string.IsNullOrEmpty(corpID)) throw new Exception("没有corpid");
            if (string.IsNullOrEmpty(corpSecret)) throw new Exception("没有corpsecret");
            Dictionary<string, object> args = new Dictionary<string, object>();
            args["corpid"] = corpID;
            args["corpsecret"] = corpSecret;
            string url = UrlConfig.GetToken;
            string json = HttpHelper.Get(url, args);
            var token = JsonHelper.Deserialize<GetTokenResponse>(json);
            return token.access_token;
        }
    }
}
