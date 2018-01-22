using System;
using System.Threading.Tasks;

namespace Bzway.Server.Core
{
    public interface IHttpServer : IDisposable
    {
        IHttpServer Use(Func<IHttpContext, Task> process);
        void Start();
        void Restart();
        void Stop();
    }
}