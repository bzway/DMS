using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bzway.Server.Wechat
{
    public class WebServer
    {
        List<Func<WechatContext, object>> processorList;
        public WebServer()
        {
            this.processorList = new List<Func<WechatContext, object>>();
        }
        public WebServer Use(Func<WechatContext, object> process)
        {
            this.processorList.Add(process);
            return this;
        }

        public void Run()
        {
            Startup.ProcessorList = this.processorList;

            try
            {
                var host = new WebHostBuilder()
                            .UseKestrel((options) =>
                            {
                                options.AddServerHeader = false;
                            })
                            .UseContentRoot(Directory.GetCurrentDirectory())
                            .UseStartup<Startup>()
                            .Build();
                host.Run();
            }
            catch { }
        }

        class Startup
        {
            public static List<Func<WechatContext, object>> ProcessorList;
            public Startup(IHostingEnvironment env)
            {
            }


            public void ConfigureServices(IServiceCollection services)
            {
            }

            public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
            {
                app.UseMiddleware<MessageProcessorMiddleware>(ProcessorList);
                app.Run(async context =>
                {
                    await context.Response.WriteAsync("");
                });
            }
        }
        class MessageProcessorMiddleware
        {

            private readonly RequestDelegate next;
            private readonly List<Func<WechatContext, object>> messageProcessor;
            public MessageProcessorMiddleware(RequestDelegate next, List<Func<WechatContext, object>> messageProcessor)
            {
                this.next = next;
                this.messageProcessor = messageProcessor;
            }
            public async Task Invoke(HttpContext context)
            {
                WechatContext wechatContext = new WechatContext(context);
                if (!wechatContext.Signatured)
                {
                    await context.Response.WriteAsync(string.Empty);
                }
                var echoString = wechatContext.Echo();
                if (!string.IsNullOrEmpty(echoString))
                {
                    await context.Response.WriteAsync(echoString);
                }
                wechatContext.Echo();
                var result = this.Process(wechatContext);
                if (result.Wait(4900))
                {
                    await context.Response.WriteAsync(result.Result.ToString());
                }
                await next(context);
            }
            Task<object> Process(WechatContext context)
            {

                Task<object> task = new Task<object>(() =>
                {
                    foreach (var item in this.messageProcessor)
                    {
                        var result = item(context);
                        if (result != null)
                        {
                            return result;
                        }
                    }
                    return string.Empty;
                });
                task.Start();
                return task;
            }
        }
    }
}
