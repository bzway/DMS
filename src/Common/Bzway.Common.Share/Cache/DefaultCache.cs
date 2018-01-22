using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bzway.Common.Share
{
    public class DefaultCache : ICacheManager
    {
        private readonly IMemoryCache cache;
        private IList<string> keys = new List<string>();

        public DefaultCache()
        {
            var options = new MemoryCacheOptions();
            this.cache = new MemoryCache(options);
        }
        public T Get<T>(string key, Func<T> call, int timeOut = 0)
        {
            if (keys.Contains(key))
            {
                T result;
                if (this.cache.TryGetValue<T>(key, out result))
                {
                    return result;
                }
            }
            var obj = call();
            keys.Add(key);
            this.Set(key, obj, timeOut);
            return obj;
        }

        public T Get<T>(string key)
        {
            if (keys.Contains(key))
            {
                T result;
                if (this.cache.TryGetValue<T>(key, out result))
                {
                    return result;
                }
            }
            return default(T);
        }
        public IList<string> GetAllKey()
        {
            return this.keys;
        }

        public bool IsSet(string key)
        {
            return this.keys.Contains(key);
        }

        public bool Remove(string key = "")
        {
            try
            {
                this.keys.Remove(key);
                this.cache.Remove(key);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Set(string key, object value, int timeOut = 0)
        {
            if (!keys.Contains(key))
            {
                keys.Add(key);
            }

            if (timeOut > 0)
            {
                var absoluteExpiration = TimeSpan.FromSeconds(timeOut);
                this.cache.Set<object>(key, value, absoluteExpiration);
            }
            else
            {
                this.cache.Set<object>(key, value);
            }
        }
    }
}