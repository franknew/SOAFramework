using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace SOAFramework.Service.Core
{
    public class Client
    {
        public Client()
        {
            PostData = new Dictionary<string, string>();
        }

        public string Url { get; set; }

        public object PostData { get; set; }

        public string Post()
        {
            if (string.IsNullOrEmpty(Url))
            {
                throw new Exception("请设置Url！");
            }
            string result = null;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Url);
            return result;
        }
    }
}
