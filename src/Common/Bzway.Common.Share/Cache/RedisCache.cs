using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
namespace Bzway.Common.Share
{
    public class RedisCacheManager : ICacheManager
    {
        private readonly IDatabase db;
        private readonly IServer server;
        public RedisCacheManager()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
      
            this.server = redis.GetServer("localhost" , 6379);
            this.db = redis.GetDatabase();
        }
        public T Get<T>(string key, Func<T> call, int timeOut = 0)
        {
            if (this.db.KeyExists(key))
            {
                var val = this.db.StringGet(key);
                return JsonConvert.DeserializeObject<T>(val);
            }
            var obj = call();
            var value = JsonConvert.SerializeObject(obj);
            if (timeOut > 0)
            {
                this.db.StringSet(key, value, TimeSpan.FromSeconds(timeOut));
            }
            else
            {
                this.db.StringSet(key, value);
            }
            return obj;
        }
        public T Get<T>(string key)
        {
            if (this.db.KeyExists(key))
            {
                var val = this.db.StringGet(key);
                return JsonConvert.DeserializeObject<T>(val);
            }
            return default(T);
        }
        public IList<string> GetAllKey()
        {
            List<string> list = new List<string>();
            foreach (var item in this.server.Keys())
            {
                list.Add(item);
            }
            return list;
        }

        public bool IsSet(string key)
        {
            return this.db.KeyExists(key);
        }

        public bool Remove(string key = "")
        {
            return this.db.KeyExpire(key, TimeSpan.FromDays(-1));
        }
        public void Set(string key, object value, int timeOut = 0)
        {
            var val = JsonConvert.SerializeObject(value);
            if (timeOut > 0)
            {
                this.db.StringSet(key, val, TimeSpan.FromSeconds(timeOut));
            }
            else
            {
                this.db.StringSet(key, val);
            }
        }
    }
}
