using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading;

namespace MicroService.Library
{
    public class HttpServerContext
    {
        private HttpServerContext _current;
        private static IDictionary<string, HttpServerContext> _contextes = new ConcurrentDictionary<string, HttpServerContext>();

        public HttpListenerContext HttpContext { get; set; }
        public ServiceModelInfo ServiceInfo { get; set; }
        public Dictionary<string, object> Args { get; set; }

        public static HttpServerContext Current
        {
            [SecurityCritical]
            get
            {
                return _contextes[Thread.CurrentContext.ContextID.ToString()];


            }
        }

        public static void AddContext(string key, HttpServerContext context)
        {
            _contextes.Add(key, context);
        }

        public static void RemoveContext(string key)
        {
            if (_contextes.ContainsKey(key)) _contextes.Remove(key);
        }
    }
}
