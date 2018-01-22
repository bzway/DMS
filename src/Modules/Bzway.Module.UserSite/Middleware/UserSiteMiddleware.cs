using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bzway.Framework.Core
{


    public class UserSiteMiddleware : OwinMiddleware
    {
        public UserSiteMiddleware(OwinMiddleware next)
            : base(next)
        {
        }
        public async override Task Invoke(IOwinContext context)
        {
            context.Set<SiteManager>("SiteManager", new SiteManager(context));
            await Next.Invoke(context);

        }
    }
}