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

        public BzwayPrincipal(IHttpContextAccessor contextAccessor)
        {
            this.httpContext = contextAccessor.HttpContext;
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
                        var user = CacheManager.Default.RedisCacheProvider.Get<UserModel>(key);
                        this.identity = new UserIdentity() { User = user };
                    }
                    else
                    {
                        this.identity = new UserIdentity();
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


            var userKey = "user_token:" + this.identity.User.Id;

            //根据用户Id得到token key
            string tokenKey;
            if (CacheManager.Default.RedisCacheProvider.IsSet(userKey))
            {
                tokenKey = CacheManager.Default.RedisCacheProvider.Get<string>(userKey);
            }
            else
            {
                tokenKey = "http_session:" + Guid.NewGuid().ToString("N");
                CacheManager.Default.RedisCacheProvider.Set(userKey, tokenKey, timeOutInSeconds);
            }
            //记录tokenKey
            CacheManager.Default.RedisCacheProvider.Set(tokenKey, this.identity.User, timeOutInSeconds);
            //写入token
            this.httpContext.Response.Cookies.Append(SessionKey, tokenKey, new CookieOptions()
            {
                Expires = DateTimeOffset.Now.AddSeconds(timeOutInSeconds),
                HttpOnly = true,
            });
        }
    }
}