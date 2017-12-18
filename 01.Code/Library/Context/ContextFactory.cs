using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SOAFramework.Library
{
    public class ContextFactory
    {
        private static IDictionary<string, IDictionary<string, object>> _context = new ConcurrentDictionary<string, IDictionary<string, object>>();

        public static IDictionary<string, object> Current
        {
            get
            {
                IDictionary<string, object> session = null;
                var key = Thread.CurrentThread.ManagedThreadId.ToString();
                session = new Dictionary<string, object>();
                if (_context.ContainsKey(key)) session = _context[key];
                else _context[key] = session;
                return session;
            }
        }

        public static IDictionary<string, IDictionary<string, object>> Context
        {
            get
            {
                return _context;
            }
        }

        public static void RemoveCurrent()
        {
            var key = Thread.CurrentThread.ManagedThreadId.ToString();
            if (_context.ContainsKey(key)) _context.Remove(key);
        }
    }
}
