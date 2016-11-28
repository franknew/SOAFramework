using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.WeiXin.Api
{
    public class UserApi
    {

        /// <summary>
        /// 新建用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public AddActionResponse Create(User user)
        {
            User_CreateRequest request = new User_CreateRequest();
            request.AccessToken = ApiHelper.GetTokenFromCache();
            request.PostData = user;
            return WeiXinSDK.Instance.Execute(request);
        }

        public WeiXinBaseResponse Update(User user)
        {
            User_UpdateRequest request = new User_UpdateRequest();
            request.AccessToken = ApiHelper.GetTokenFromCache();
            request.PostData = user;
            return WeiXinSDK.Instance.Execute(request);
        }

        public WeiXinBaseResponse Delete(string userid)
        {
            User_DeleteRequest request = new User_DeleteRequest();
            request.AccessToken = ApiHelper.GetTokenFromCache();
            request.QueryString = new User_DeleteQueryString { userid = userid };
            request.RequestType = RequestType.Get;
            return WeiXinSDK.Instance.Execute(request);
        }

        public User_GetResponse Get(User_GetQueryString querystring)
        {
            User_GetRequest request = new User_GetRequest();
            request.AccessToken = ApiHelper.GetTokenFromCache();
            request.QueryString = querystring;
            request.RequestType = RequestType.Get;
            return WeiXinSDK.Instance.Execute(request);
        }

        public User_GetUserInfoResponse GetUserInfo(User_GetUserInfoQueryString querystring)
        {
            User_GetUserInfoRequest request = new User_GetUserInfoRequest();
            request.AccessToken = ApiHelper.GetTokenFromCache();
            request.QueryString = querystring;
            request.RequestType = RequestType.Get;
            return WeiXinSDK.Instance.Execute(request);
        }
    }
}
