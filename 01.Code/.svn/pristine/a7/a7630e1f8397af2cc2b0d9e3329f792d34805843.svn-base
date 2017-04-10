using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.WeiXin.Api
{
    public class DepartmentApi
    {
        public AddActionResponse Create(Department department)
        {
            Department_CreateRequest request = new Department_CreateRequest();
            request.AccessToken = ApiHelper.GetTokenFromCache();
            request.PostData = department;
            return WeiXinSDK.Instance.Execute(request);
        }

        public WeiXinBaseResponse Update(Department department)
        {
            Department_UpdateRequest request = new Department_UpdateRequest();
            request.AccessToken = ApiHelper.GetTokenFromCache();
            request.PostData = department;
            return WeiXinSDK.Instance.Execute(request);
        }

        public WeiXinBaseResponse Delete(string id)
        {
            Department_DeleteRequest request = new Department_DeleteRequest();
            request.AccessToken = ApiHelper.GetTokenFromCache();
            request.QueryString = new CommonIDQueryString { id = id };
            request.RequestType = RequestType.Get;
            return WeiXinSDK.Instance.Execute(request);
        }

        /// <summary>
        /// 部门名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Department_ListResponse List(string name)
        {
            Department_ListRequest request = new Department_ListRequest();
            request.AccessToken = ApiHelper.GetTokenFromCache();
            request.RequestType = RequestType.Get;
            request.QueryString = new CommonIDQueryString { id = name };
            return WeiXinSDK.Instance.Execute(request);
        }
    }
}
