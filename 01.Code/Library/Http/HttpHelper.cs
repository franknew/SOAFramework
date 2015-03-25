using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Web;
using System.IO;

namespace SOAFramework.Library
{
    public class HttpHelper
    {
        public static string Post(string url, byte[] data, int timeout = -1)
        {
            WebRequest request = WebRequest.Create(url);
            request.ContentType = "application/json"; 
            request.Method = "POST";
            request.Credentials = CredentialCache.DefaultCredentials;
            request.ContentLength = data.Length;
            request.Timeout = timeout;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader responseReader = new StreamReader(responseStream);
            return responseReader.ReadToEnd();
        }

        public static string Get(string url, int timeout = -1)
        {
            WebRequest request = WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = timeout;
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader responseReader = new StreamReader(responseStream);
            return responseReader.ReadToEnd();
        }
    }
}
