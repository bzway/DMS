using Microsoft.Owin;
using Owin;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Web;



namespace OpenData.Framework.Common
{
    using System.Diagnostics;
    using AppFunc = Func<IDictionary<string, object>, Task>;
    public class LogOwinMiddleware : OwinMiddleware
    {

        public LogOwinMiddleware(OwinMiddleware next)
            : base(next)
        {
        }

        public async override Task Invoke(IOwinContext context)
        {
            var sessionId = context.Request.Cookies["_session"];
            await Next.Invoke(context);
        }
    }





    public class LogMiddleware
    {

        private readonly AppFunc next;

        public LogMiddleware(AppFunc next)
        {
            this.next = next;
        }

        public async Task Invoke(IDictionary<string, object> env)
        {
            if (env.ContainsKey("_culture"))
            {
                //var aaa = "";
            }
            Console.WriteLine("LogMiddleware Start.");
            await next(env);
            Console.WriteLine("LogMiddleware End.");
        }
    }

    public class FrontPageMiddleware : OwinMiddleware
    {

        public FrontPageMiddleware(OwinMiddleware next)
            : base(next)
        {
        }

        public async override Task Invoke(IOwinContext context)
        {

#if TRACE
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            context.Response.Write("<!--Start-->");
#endif 

            await Next.Invoke(context);

#if TRACE

            stopwatch.Stop();
            context.Response.Write(string.Format("<!--End:{0}ms-->", stopwatch.ElapsedMilliseconds));
#endif
        }

    }

    public class XRequest : HttpRequestBase
    {
        public XRequest()
        {

        }
    }
    public class XResponse : HttpResponseBase
    {

    }
    public class XContext : HttpContextBase
    {
    }
}