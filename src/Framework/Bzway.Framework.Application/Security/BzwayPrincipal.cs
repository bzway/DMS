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
        private const string SessionKey = "USR";
        private readonly HttpContext httpContext;
        private readonly ITokenService tokenService;

        public BzwayPrincipal(IHttpContextAccessor contextAccessor, ITokenService tokenService)
        {
            this.httpContext = contextAccessor.HttpContext;
            this.tokenService = tokenService;
        }
        private ClaimsIdentity identity;
        public override IIdentity Identity
        {
            get
            {
                if (this.identity == null)
                {
                    var token = string.Empty;
                    if (this.httpContext.Request.Cookies.ContainsKey(SessionKey))
                    {
                        token = this.httpContext.Request.Cookies[SessionKey];
                    }
                    else if (this.httpContext.Request.Query.ContainsKey(SessionKey))
                    {
                        token = this.httpContext.Request.Query[SessionKey];
                    }
                    else
                    {
                        token = this.httpContext.Request.Headers[SessionKey].FirstOrDefault();
                    }
                    ClaimsIdentity identity = this.tokenService.GetUserToken(token).Data;
                    if (identity == null)
                    {
                        this.identity = new ClaimsIdentity();
                    }
                    else
                    {
                        this.identity = identity;
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
            var token = this.tokenService.GenerateUserAccessToken(identity).Data;
            //写入token
            this.httpContext.Response.Cookies.Append(SessionKey, token.Token, new CookieOptions()
            {
                Expires = DateTimeOffset.Now.AddSeconds(token.ExpiredIn),
                HttpOnly = true,
            });
        }
    }
}