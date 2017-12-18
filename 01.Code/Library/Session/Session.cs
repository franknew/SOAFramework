using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SOAFramework.Library
{
    public class Session
    {
        private static IDictionary<string, IDictionary<string, object>> _session = new ConcurrentDictionary<string, IDictionary<string, object>>();

        public static IDictionary<string, object> Current
        {
            get
            {
                IDictionary<string, object> session = null;
                try
                {

                    var key = Thread.CurrentThread.ManagedThreadId.ToString();
                    session = new Dictionary<string, object>();
                    if (_session.ContainsKey(key)) session = _session[key];
                    else _session[key] = session;
                }
                catch (Exception ex)
                { }
                return session;
            }
        }

        public static void RemoveCurrent()
        {
            try
            {
                var key = Thread.CurrentThread.ManagedThreadId.ToString();
                if (_session.ContainsKey(key)) _session.Remove(key);
            }
            catch (Exception ex)
            { }
        }
    }
}
