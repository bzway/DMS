using System.Security.Principal;
using System.Linq;
using System.Collections.Generic;
using Bzway.Common.Utility;
using System.Security.Claims;
using Bzway.Common.Share;
using System;
using Microsoft.AspNetCore.Http;
using System.IO;
using Newtonsoft.Json;

namespace Bzway.Framework.Application
{
    /// <summary>
    /// 授权Model
    /// </summary>
    public class AuthorizationResponseModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiredIn { get; set; }
    }
    public class BzwayPrincipal : ClaimsPrincipal
    {
        public static readonly string SessionKey = "USR";
        private readonly IDictionary<string, string> cookies;
        private readonly HttpContext httpContext;
        private readonly ICacheManager cache;

        public BzwayPrincipal(IHttpContextAccessor contextAccessor)
        {
            this.httpContext = contextAccessor.HttpContext;
            this.cache = AppEngine.GetService<ICacheManager>("Redis");

            this.cookies = new Dictionary<string, string>();
            foreach (var item in this.httpContext.Request.Cookies.Where(m => m.Key == SessionKey))
            {
                this.cookies.Add(item.Key, item.Value);
            }
            foreach (var item in this.httpContext.Request.Headers.Where(m => m.Key == SessionKey))
            {
                this.cookies.Add(item.Key, item.Value);
            }
        }
        private UserIdentity identity;
        public override IIdentity Identity
        {
            get
            {
                if (this.identity == null)
                {
                    if (cookies.ContainsKey(SessionKey))
                    {
                        var key = cookies[SessionKey];
                        this.identity = cache.Get<UserIdentity>(key);
                    }
                    if (this.identity == null)
                    {
                        this.identity = new UserIdentity() { Id = string.Empty, Roles = string.Empty };
                    }
                }
                return this.identity;
            }
        }
        public override string ToString()
        {
            return this.identity.Name;
        }
        public override void AddIdentity(ClaimsIdentity identity)
        {
            this.identity = identity as UserIdentity;
            if (this.identity == null)
            {
                base.AddIdentity(identity);
                return;
            }
            var timeOutInSeconds = 1000;
            var value = JsonConvert.SerializeObject(identity);

            var userKey = "user_token:" + this.identity.Id;

            //根据用户Id得到token key
            string tokenKey;
            if (this.cache.IsSet(userKey))
            {
                tokenKey = this.cache.Get<string>(userKey);
            }
            else
            {
                tokenKey = "http_session:" + Guid.NewGuid().ToString("N");
                this.cache.Set(userKey, tokenKey, timeOutInSeconds);
            }
            //记录tokenKey
            this.cache.Set(tokenKey, value, timeOutInSeconds);
            //写入token
            this.httpContext.Response.Cookies.Append(SessionKey, tokenKey, new CookieOptions()
            {
                Expires = DateTimeOffset.Now.AddSeconds(timeOutInSeconds),
                HttpOnly = true,
            });
        }
    }
}