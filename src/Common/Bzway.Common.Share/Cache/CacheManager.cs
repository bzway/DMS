using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bzway.Common.Share
{


    public class CacheManager
    {
        #region ctor
        static readonly Lazy<CacheManager> lazy = new Lazy<CacheManager>(() => { return new CacheManager(); });
        public static CacheManager Default
        {
            get
            {
                return lazy.Value;
            }
        }
        private CacheManager()
        {
        }
        #endregion
        public ICache DefaultCacheProvider
        {
            get
            {
                return DefaultCache.Default;
            }
        }
        public ICache RedisCacheProvider
        {
            get
            {
                return RedisCache.Default;
            }
        }
    }
}