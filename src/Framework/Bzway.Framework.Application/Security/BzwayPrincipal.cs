using System.Security.Principal;
using System.Linq;
using System.Collections.Generic;
using Bzway.Common.Utility;
using System.Security.Claims;
using Bzway.Common.Share;
using System;
using Microsoft.AspNetCore.Http;

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
        static readonly string SessionKey = "USR";
        private readonly IDictionary<string, string> cookies;
        private readonly HttpContext httpContext;
        private readonly ICacheManager cache;

        public BzwayPrincipal(IHttpContextAccessor contextAccessor)
        {
            this.httpContext = contextAccessor.HttpContext;
            this.cache = AppEngine.GetService<ICacheManager>("Redis");

            this.cookies = new Dictionary<string, string>();
            foreach (var item in this.httpContext.Request.Cookies)
            {
                this.cookies.Add(item.Key, item.Value);
            }
        }
        public Result<AuthorizationResponseModel> SaveToken(UserIdentity value, int timeOutInSeconds)
        {
            //根据用户Id得到token
            var userKey = string.Format("userToken_{0}", value.Id);
            string token;
            if (cache.IsSet(userKey))
            {
                token = cache.Get<string>(userKey);
            }
            else
            {
                token = string.Format("tokenKey_{0}", Guid.NewGuid().ToString("N"));
                cache.Set(userKey, token, timeOutInSeconds);
            }
            cache.Set(token, value, timeOutInSeconds);
            this.httpContext.Response.Cookies.Append(SessionKey, token, new CookieOptions()
            {
                Expires = DateTimeOffset.Now.AddSeconds(timeOutInSeconds),
                HttpOnly = true,
            });
            return Result.OK<AuthorizationResponseModel>(new AuthorizationResponseModel()
            {
                ExpiredIn = timeOutInSeconds,
                RefreshToken = "",
                Token = token
            });
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
                    else
                    {
                        this.identity = new UserIdentity() { Id = string.Empty, Locked = LockType.None, Name = string.Empty, Roles = string.Empty };
                    }
                }
                return this.identity;
            }
        }
        public override string ToString()
        {
            return this.identity.Name;
        }
    }
}