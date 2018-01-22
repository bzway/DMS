
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Bzway.Server.Core
{
    public interface IHttpContext : IDisposable
    {
        object Get(string key);
        T Get<T>(string key);

        void Set(string key, object value);
    }
}