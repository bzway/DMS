using Microsoft.Owin;
using Bzway.Framework.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bzway.Framework.Core
{
    public class UserIdentityMiddleware : OwinMiddleware
    {
        public UserIdentityMiddleware(OwinMiddleware next)
            : base(next)
        {
        }
        public async override Task Invoke(IOwinContext context)
        {

            UserManager um = new UserManager(context);

            var dict = new Dictionary<string, string>();
            foreach (var item in context.Request.Cookies)
            {
                dict.Add(item.Key, item.Value);
            }
            context.Request.User = new BzwayPrincipal(dict);
            await Next.Invoke(context);
        }
    }
}