using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace WYJK.Framework.Caching
{
    public class MemoryCacheHandle : ICache
    {
        private const string DefaultName = "Minho";
        private const int ExpirationMinutes = 60;

        private readonly MemoryCache _memoryCache ;

        public MemoryCacheHandle()
        {
            _memoryCache = new MemoryCache(DefaultName);
        }
        public MemoryCacheHandle(string name)
        {
            _memoryCache = new MemoryCache(name);
        }
        public object Get(string key)
        {
            return _memoryCache.Get(key);
        }

        public T Get<T>(string key)
        {
            return (T)_memoryCache.Get(key);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public object Set(string key, Func<object> func)
        {
            object value = func();
            if (value != null)
            {
                _memoryCache.Set(key, value, DateTimeOffset.Now.AddMinutes(ExpirationMinutes));
            }
            return value;
        }

        public object Set(string key, object value)
        {
            _memoryCache.Set(key,value, DateTimeOffset.Now.AddMinutes(ExpirationMinutes));
            return value;
        }

        public object Set(string key, Func<object> func, TimeSpan duration)
        {
            object value = func();
            if (value != null)
            {
                _memoryCache.Set(key, value, DateTimeOffset.Now.AddMinutes(duration.Minutes));
            }
            return value;
        }

        public object Set(string key, object value, TimeSpan duration)
        {
            _memoryCache.Set(key, value, DateTimeOffset.Now.AddMinutes(duration.Minutes));
            return value; 
        }
    }
}
