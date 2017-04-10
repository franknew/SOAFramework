using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.WeiXin
{
    public interface IWeiXinRequest<T> where T : WeiXinBaseResponse
    {
        string GetApi();

        RequestType RequestType { get; set; }

        string AccessToken { get; set; }
    }

    public abstract class WeiXinBaseRequest<T> : IWeiXinRequest<T> where T : WeiXinBaseResponse
    {
        public abstract string GetApi();

        private RequestType requestType = RequestType.Post;
        private Dictionary<string, object> queryString = new Dictionary<string, object>();

        public string AccessToken { get; set; }

        public RequestType RequestType
        {
            get
            {
                return requestType;
            }

            set
            {
                requestType = value;
            }
        }
    }

    public enum RequestType
    {
        Get,
        Post
    }
}
