using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public class LogCacheManager<T> 
    {
        private static ConcurrentDictionary<string, ConcurrentQueue<T>> _logPool = new ConcurrentDictionary<string, ConcurrentQueue<T>>();

        public static ConcurrentQueue<T> GetOrCreate(string key)
        {
            ConcurrentQueue<T> queue = new ConcurrentQueue<T>();
            if (_logPool.ContainsKey(key)) queue = _logPool[key];
            else _logPool.TryAdd(key, queue);
            return queue;
        }

        public static bool Enqueue(T log)
        {
            foreach (var key in _logPool.Keys)
            {
                _logPool[key].Enqueue(log);
            }
            return true;
        }

        public static bool ClearAll()
        {
            _logPool.Clear();
            return true;
        }

        public static ConcurrentQueue<T> Clear(string key)
        {
            ConcurrentQueue<T> t = new ConcurrentQueue<T>();
            if (_logPool.ContainsKey(key)) _logPool.TryRemove(key, out t);
            return t;
        }
    }
}
