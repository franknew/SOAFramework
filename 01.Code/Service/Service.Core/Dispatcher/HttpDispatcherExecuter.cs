using SOAFramework.Library;
using SOAFramework.Service.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.Core
{
    [ServiceLayer(IsHiddenDiscovery = true)]
    public class HttpDispatcherExecuter : IDispatcherExecuter
    {
        public string Execute(string endPoint, string typeName, string functionName, Dictionary<string, string> args)
        {
            string executeUrl = string.Format("{0}/Execute/{1}/{2}", endPoint.TrimEnd('/'), typeName, functionName);
            string result = null;
            List<PostArgItem> list = new List<PostArgItem>();
            foreach (var key in args.Keys)
            {
                PostArgItem arg = new PostArgItem
                {
                    Key = key,
                    Value = args[key],
                };
            }
            byte[] data = Encoding.UTF8.GetBytes(JsonHelper.Serialize(list));
            result = HttpHelper.Post(executeUrl, data);
            return result;
        }
    }
}
