using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Bzway.Server.Core
{

    public class HttpServer : IHttpServer
    {
        #region Fields

        private readonly int Port;
        private readonly TcpListener Listener;
        private bool IsRunning;

        private List<Func<IHttpContext, Task>> funList = new List<Func<IHttpContext, Task>>();

        #endregion
        public HttpServer(int port)
        {
            this.IsRunning = false;
            this.Port = port;
            this.Listener = new TcpListener(IPAddress.Any, this.Port);
        }

        #region Public Methods

        Task Process(IHttpContext context)
        {
            return Task.Run(() =>
            {
                try
                {
                    Console.WriteLine("Start");
                    foreach (var item in this.funList)
                    {
                        item(context);
                    }

                    var stream = context.Get<Stream>(HttpBuilder.ResponseBody);
                    var response = new HttpResponse(stream) { Content="test" };
 
                    response.WriteResponse();
                    context.Dispose();
                    Console.WriteLine("End");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    context.Dispose();
                }
            });
        }

        public void Start()
        {
            if (this.IsRunning)
            {
                return;
            }
            this.IsRunning = true;
            this.Listener.Start();
            Task.Run(() =>
            {
                while (this.IsRunning)
                {
                    var client = this.Listener.AcceptTcpClientAsync().Result;
                    var context = new HttpContext(client);
                    this.Process(context);
                }
            });
        }

        public void Dispose()
        {
            this.Stop();
        }

        public IHttpServer Use(Func<IHttpContext, Task> process)
        {
            this.funList.Add(process);
            return this;
        }

        public void Restart()
        {
            this.Stop();
            this.Start();
        }

        public void Stop()
        {
            this.Listener.Stop();
            this.IsRunning = false;

        }

        #endregion

    }
}



