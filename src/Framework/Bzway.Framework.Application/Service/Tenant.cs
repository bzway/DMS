using Bzway.Database.Core;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Bzway.Framework.Application.Entity;
using Microsoft.AspNetCore.Http;
using Bzway.Common.Share;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Principal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authorization;

namespace Bzway.Framework.Application
{
    /// <summary>
    /// GrantRequest service
    /// </summary>
    internal class Tenant : ITenant
    {
        #region ctor        
        public Tenant(IHttpContextAccessor contextAccessor, IPrincipal principal)
        {
            this.Context = contextAccessor.HttpContext;
            this.Context.User = principal as ClaimsPrincipal;
            var cache = AppEngine.GetService<ICacheManager>();
            var domain = this.Context.Request.Host.Value;
            this.Site = cache.Get<Site>("Site.Domain." + domain, () =>
            {
                var db = SystemDatabase.GetDatabase().Entity<Site>();
                return db.Query().Where(m => m.Domains.Contains(domain)).FirstOrDefault();
            });
        }
        #endregion
        public HttpContext Context { get; private set; }
        public Site Site { get; private set; }
    }


    public class TokenAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public string TokenName => "Token";

        public string RedirectUri => "/account/login";
        public double ExpiresIn => 120;
        public bool? AllowRefresh => true;
        public bool IsPersistent => true;
    }
    public class TokenAuthenticationHandler : AuthenticationHandler<TokenAuthenticationSchemeOptions>, IAuthenticationSignInHandler, IAuthenticationSignOutHandler
    {
        public TokenAuthenticationHandler(ITenant tenant, IOptionsMonitor<TokenAuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {

        }

        public Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
        {
            return Task.Run(() =>
            {
                this.Response.Cookies.Append(this.Options.TokenName, properties.GetTokenValue(this.Options.TokenName), new CookieOptions()
                {
                    Expires = DateTimeOffset.Now.AddMinutes(this.Options.ExpiresIn),
                    HttpOnly = true,
                });
            });
        }

        public Task SignOutAsync(AuthenticationProperties properties)
        {
            return Task.Run(() =>
            {
                this.Response.Cookies.Delete(this.Options.TokenName);
            });
        }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            return Task.Run<AuthenticateResult>(() =>
            {
                var principal = this.Context.User;
                var properties = new AuthenticationProperties()
                {
                    AllowRefresh = this.Options.AllowRefresh,
                    ExpiresUtc = DateTimeOffset.Now.AddMinutes(this.Options.ExpiresIn),
                    IsPersistent = this.Options.IsPersistent,
                    IssuedUtc = DateTimeOffset.Now,
                    RedirectUri = this.Options.RedirectUri,
                };
                return AuthenticateResult.Success(new AuthenticationTicket(principal, null, this.Scheme.Name));
            });
        }
    }
}