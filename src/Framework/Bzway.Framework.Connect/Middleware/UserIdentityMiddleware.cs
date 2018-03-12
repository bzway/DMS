using Bzway.Framework.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bzway.Framework.Connect
{
    public class UserIdentityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public UserIdentityMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<UserIdentityMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            var dict = new Dictionary<string, string>();
            foreach (var item in context.Request.Cookies)
            {
                dict.Add(item.Key, item.Value);
            }
            await _next.Invoke(context);
        }
    }
}