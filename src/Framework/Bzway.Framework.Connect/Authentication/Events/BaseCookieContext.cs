using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Bzway.Framework.Connect.Authentication
{
    public class BaseCookieContext : BaseContext
    {
        public BaseCookieContext(
           HttpContext context,
            CookieAuthenticationOptions options)
            : base(context)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Options = options;
        }

        public CookieAuthenticationOptions Options { get; }
    }
}
