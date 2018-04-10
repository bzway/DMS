using Bzway.Common.Collections;
using System;
using System.Reflection;
using System.Threading;
using Bzway.Common.Utility;
using Bzway.Common.Share;
using Bzway.Module.Wechat.Interface;
using Autofac;
using Bzway.Common.Script;

namespace Bzway.Server.Wechat
{
    public static class Exetension
    {
        public static WebServer UseKeyWord(this WebServer app)
        {
            app.Use(ProcessKeyWord);
            return app;
        }

        public static WebServer UseBaseProcess(this WebServer app)
        {
            app.Use(ProcessBase);
            return app;
        }

        public static WebServer UseMvc(this WebServer app)
        {
            app.Use(ProcessKeyMvc);
            return app;
        }
        static string ProcessKeyWord(WechatContext context)
        {
            IWechatService service = AppEngine.Default.GetService<IWechatService>();
            var list = service.GetWechatResponse(context.openId, Module.Wechat.Entity.SearchType.None, context.CurrentOfficialAccount.Id);
            return "Keyword";
        }
        static Assembly pluginAssembly;
        static object obj = new object();
        static string ProcessKeyMvc(WechatContext context)
        {
            if (pluginAssembly == null)
            {
                lock (obj)
                {
                    if (pluginAssembly == null)
                    {
                        var d = @"D:\Work\OpenData.Core\Server\WechatMessageServer\Process";
                        Generator generator = new Generator();
                        pluginAssembly = generator.Generate(d);
                    }
                }
            }
            foreach (var item in pluginAssembly.GetTypes())
            {
                var plugin = (DynamicView)Activator.CreateInstance(item);
                plugin.ExecuteAsync().GetAwaiter().GetResult();
                if (!string.IsNullOrEmpty(plugin.Content.ToString()))
                {
                    return plugin.Content.ToString();
                }
            }
            return "Mvc";
        }
        static string ProcessBase(WechatContext context)
        {
            if (!context.Signatured)
            {
                return string.Empty;
            }
            //todo
            return null;
        }
    }
}