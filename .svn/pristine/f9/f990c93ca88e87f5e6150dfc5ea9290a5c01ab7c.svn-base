using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AustinHarris.JsonRpc;

namespace MicroService.Library
{
    public class FilterHandler
    {
        private static ConcurrentDictionary<string, FilterHandler> _sessionHandlers = new ConcurrentDictionary<string, FilterHandler>();
        private static string _defaultSessionId;
        public ConcurrentDictionary<string, List<IFilterAttribute>> MetaData { get; set; }
        private IDictionary<string, Delegate> Handlers { get; set; }

        public FilterHandler(string sessionID = null)
        {
            if (string.IsNullOrEmpty(sessionID)) _defaultSessionId = Guid.NewGuid().ToString();
            else _defaultSessionId = sessionID;
            sessionID = _defaultSessionId;
            MetaData = new ConcurrentDictionary<string, List<IFilterAttribute>>();
            //_sessionHandlers[sessionID] = new FilterHandler(sessionID);
        }

        public static FilterHandler GetSessionHandler(string sessionID)
        {
            return _sessionHandlers.GetOrAdd(sessionID, new FilterHandler(sessionID));
        }

        public static FilterHandler GetSessionHandler()
        {
            return GetSessionHandler(_defaultSessionId);
        }

        public static void DestroySession(string sessionId)
        {
            FilterHandler h;
            _sessionHandlers.TryRemove(sessionId, out h);
            h.Handlers.Clear();
            h.MetaData.Clear();
        }

        public bool Register(string key, Delegate handle)
        {
            var result = false;

            if (!this.Handlers.ContainsKey(key))
            {
                this.Handlers.Add(key, handle);
            }

            return result;
        }

        public void UnRegister(string key)
        {
            List<IFilterAttribute> value = new List<IFilterAttribute>();
            this.Handlers.Remove(key);
            MetaData.TryRemove(key, out value);
        }

        public HttpFilterResult HandleOnActionExecuting(string serviceName, HttpServerContext context)
        {
            HttpFilterResult result = new HttpFilterResult();
            if (!this.MetaData.ContainsKey(serviceName)) return result;
            var filters = this.MetaData[serviceName];
            for (int i = 0; i < filters.Count; i++)
            {
                result = filters[i].OnActionExecuting(context);
                if (result.IsError) break;
            }
            return result;
        }

        public HttpFilterResult HandleOnActionExecuted(string serviceName, HttpServerContext context)
        {
            HttpFilterResult result = new HttpFilterResult();
            if (!this.MetaData.ContainsKey(serviceName))return result;
            var filters = this.MetaData[serviceName];
            for (int i = filters.Count - 1; i >= 0; i--)
            {
                result = filters[i].OnActionExecuted(context);
                if (result.IsError) break;
            }
            return result;
        }

        public HttpFilterResult HandleOnException(string serviceName, HttpServerContext context, Exception ex)
        {
            HttpFilterResult result = new HttpFilterResult();
            if (!this.MetaData.ContainsKey(serviceName)) return result;
            var filters = this.MetaData[serviceName];
            filters.ForEach(t =>
            {
                result = t.OnException(context, ex);
                if (result.IsError) return;
            });
            return result;
        }
    }
}
