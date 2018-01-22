using Microsoft.Owin;
using Owin;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Web;



namespace OpenData.Mvc
{
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
}