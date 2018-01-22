using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bzway.Common.Share
{
    public interface ICacheManager
    {
        T Get<T>(string key);
        T Get<T>(string key, Func<T> call, int timeOut = 0);
        IList<string> GetAllKey();
        bool Remove(string key = "");
        bool IsSet(string key);
        void Set(string key, object value, int timeOut = 0);
    }
}