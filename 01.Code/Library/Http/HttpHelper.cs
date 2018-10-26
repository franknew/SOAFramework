using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Web;
using System.IO;
using System.Reflection;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace SOAFramework.Library
{
    public class HttpHelper
    {
        public static string Post(string url, byte[] data, int timeout = -1, string contentType = "application/json",
            NetworkCredential credential = null, bool https = false, IDictionary<string, string> header = null, IDictionary<string, string> cookieDic = null, string method= "POST")
        {
            HttpWebRequest request = BuildRequest(url, contentType, timeout, https, header, cookieDic, method);
            if (https) ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            request.ServicePoint.Expect100Continue = false;
            if (credential != null)
            {
                CredentialCache cache = new CredentialCache();
                cache.Add(new Uri(url), "Basic", credential);
                string code = "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(credential.UserName + ":" + credential.Password));
                request.Credentials = cache;
                request.Headers.Add(HttpRequestHeader.Authorization, code);
            }
            Stream requestStream = request.GetRequestStream();
            request.ContentLength = data.Length;
            requestStream.Write(data, 0, data.Length);
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader responseReader = new StreamReader(responseStream);
            requestStream.Close();
            return responseReader.ReadToEnd();
        }

        public static string Put(string url, byte[] data, int timeout = -1, string contentType = "application/json",
            NetworkCredential credential = null, bool https = false, IDictionary<string, string> header = null, IDictionary<string, string> cookieDic = null)
        {
            return Post(url, data, timeout, contentType, credential, https, header, cookieDic, "PUT");
        }

        public static string Get(string url, IDictionary<string, object> args = null, int timeout = -1, bool https = false, 
            IDictionary<string, string> header = null, IDictionary<string, string> cookieDic = null)
        {
            string fullurl = CombineUrl(url, args);
            var request = BuildRequest(fullurl, "application/x-www-form-urlencoded", timeout, https, header, cookieDic, "GET");
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader responseReader = new StreamReader(responseStream);
            return responseReader.ReadToEnd();
        }

        private static HttpWebRequest BuildRequest(string url, string contentType = "application/json", int timeout = -1, bool https = false, 
            IDictionary<string, string> header = null, IDictionary<string, string> cookieDic = null, string method = "POST")
        {
            if (https) ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = timeout;
            request.ContentType = contentType;
            request.Method = method;
            if (header != null)
            {
                foreach (var key in header.Keys)
                {
                    if (!string.IsNullOrEmpty(header[key])) request.Headers.Add(key, header[key]);
                }
            }
            if (cookieDic != null)
            {
                if (request.CookieContainer == null) request.CookieContainer = new CookieContainer();
                foreach (var key in cookieDic.Keys)
                {
                    if (!string.IsNullOrEmpty(cookieDic[key]))
                    {
                        Cookie cookie = new Cookie(key, cookieDic[key]);
                        request.CookieContainer.Add(cookie);
                    }
                }
            }
            return request;
        }

        public static string CombineUrl(string url, IDictionary<string, object> args = null)
        {
            StringBuilder urlbuilder = new StringBuilder();
            urlbuilder.Append(url);
            if (args == null) return urlbuilder.ToString().TrimEnd('&');
            if (args != null && args.Keys.Count > 0)
            {
                if (!urlbuilder.ToString().Contains("?")) urlbuilder.Append("?");
                else urlbuilder.Append("&");
                foreach (var key in args.Keys)
                {
                    if (args[key] != null) urlbuilder.AppendFormat("{0}={1}&", key, args[key].ToString());
                }
            }
            return urlbuilder.ToString().TrimEnd('&');
        }

        public static string CombineUrl(string url, object args = null)
        {
            StringBuilder urlbuilder = new StringBuilder();
            urlbuilder.Append(url);
            if (args == null) return urlbuilder.ToString().TrimEnd('&');
            var properties = args.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            if (properties != null && properties.Length > 0)
            {
                if (!urlbuilder.ToString().Contains("?")) urlbuilder.Append("?");
                else urlbuilder.Append("&");
                foreach (var p in properties)
                {
                    object value = p.GetValue(args, null);
                    if (value != null) urlbuilder.AppendFormat("{0}={1}&", p.Name, value.ToString());
                }
            }
            return urlbuilder.ToString().TrimEnd('&');
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受     
        }
    }
}
