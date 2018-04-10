using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bzway.Module.Wechat.Interface;
using Microsoft.Extensions.Logging;
using System.Threading;
using Bzway.Framework.Application;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Bzway.Common.Share.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;

namespace Bzway.Sites.BackOffice.Controllers
{

    public class AccountController : BaseController<HomeController>
    {
        #region ctor
        public AccountController(ITenant tenant, ILoggerFactory loggerFactory) : base(loggerFactory, tenant) { }
        #endregion

        public async Task<IActionResult> Login(string code, string state, string url)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.Redirect(url);
            }
            //如果没有请求码
            if (string.IsNullOrEmpty(code))
            {
                //get request code
                if (string.IsNullOrEmpty(url))
                {
                    url = "/";
                }
                var authorizedUrl = this.Request.Path + "?url=" + url;
                var redirecturl = this.Request.Path + "?code=123&state=123&url=" + HttpUtility.UrlEncode(authorizedUrl);
                return this.Redirect(redirecturl);
            }
            //如果状态码有误
            if (!string.Equals(state, "123"))
            {
                return Redirect("/Error/Index");
            }
            //request access token
            string appId = "appId";
            string secretKey = "secretKey";
            string grantType = "";
            var token = "/Coonect/AccessToken";//todo get access token from auth server according request code.
            if (string.IsNullOrEmpty(token))
            {
                return Redirect("/Error/Index");
            }
            //request user profile
            var roles = "Admin,SuperUser";//todo get user profile according to access token.
            ClaimsPrincipal principal = new ClaimsPrincipal();
            var identity = new ClaimsIdentity(OpenAuthenticationOptions.DefaultSchemeName);
            identity.AddClaim(new Claim(ClaimTypes.Name, "SuperUser"));
            foreach (var item in roles.Split(','))
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, item));
            }

            principal.AddIdentity(identity);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                RedirectUri = url,
                ExpiresUtc = DateTime.UtcNow.AddDays(30),
            };
            return this.SignIn(principal, properties, OpenAuthenticationOptions.DefaultSchemeName);
        }
        public IActionResult LogOff(string code, string state, string url)
        {
            return this.SignOut(OpenAuthenticationOptions.DefaultSchemeName);
        }
    }
}
