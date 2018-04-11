using Microsoft.AspNetCore.Http;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Net.Http.Headers;
using Bzway.Common.Utility;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using System.Globalization;

namespace Bzway.Framework.Application
{

    public class OpenAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultSchemeName = "OpenAuthentication";
        public OpenAuthenticationOptions()
        {
            this.CookieName = "USR";
            this.QueryName = this.TokenName = "token";
            this.ReturnUrlParameter = "url";
            this.LoginPath = "/account/login";
            this.AccessDeniedPath = "/home/denied";
            this.LogoutPath = "/account/logout";
        }
        public string ReturnUrlParameter { get; set; }
        public PathString AccessDeniedPath { get; set; }
        public PathString LogoutPath { get; set; }
        public PathString LoginPath { get; set; }
        public string CookieName { get; internal set; }
        public string QueryName { get; internal set; }
        public string TokenName { get; internal set; }

        internal Task<AuthenticationTicket> TicketUnprotect(string token)
        {
            return Task.Run(() =>
            {
                var item = token.Split('.');
                if (item.Length != 3)
                {
                    return null;
                }
                var header = Encoding.UTF8.GetString(Convert.FromBase64String((item[0])));
                if (!this.CookieName.Equals(header))
                {
                    return null;
                }
                var body = Encoding.UTF8.GetString(Convert.FromBase64String((item[1])));
                var footer = Encoding.UTF8.GetString(Convert.FromBase64String((item[2])));
                var sign = Cryptor.EncryptSHA1(body, this.CookieName);
                if (string.Equals(footer, sign))
                {
                    return null;
                }

                ClaimsPrincipal principal = new ClaimsPrincipal();

                foreach (var id in body.Split('\r'))
                {
                    var identity = new ClaimsIdentity(this.TokenName);

                    foreach (var claim in id.Split(';'))
                    {
                        var i = claim.IndexOf('=');
                        var type = claim.Substring(0, i);
                        var value = claim.Substring(i + 1);
                        switch (type)
                        {
                            case "role":
                                identity.AddClaim(new Claim(ClaimTypes.Role, value));
                                break;
                            case "name":
                                identity.AddClaim(new Claim(ClaimTypes.Name, value));
                                break;
                            default:
                                identity.AddClaim(new Claim(type, value));
                                break;
                        }
                    }
                    principal.AddIdentity(identity);
                    var language = identity.Claims.FirstOrDefault(m => m.Type == "Language");
                    if (language != null)
                    {
                        Thread.CurrentThread.CurrentCulture = new CultureInfo(language.Value);
                    }
                }

                return new AuthenticationTicket(principal, this.TokenName);
            });
        }
        internal Task<string> TicketProtect(ClaimsPrincipal principal)
        {
            return Task.Run(() =>
            {
                var body = string.Join('\r', principal.Identities.Select(id =>
                {
                    return string.Join(';', id.Claims.Select(m =>
                    {
                        string type;
                        switch (m.Type)
                        {
                            case ClaimTypes.Role:
                                type = "role";
                                break;
                            case ClaimTypes.Name:
                                type = "name";
                                break;
                            default:
                                type = m.Type;
                                break;
                        }
                        return type + "=" + m.Value;
                    }));
                }));
                var playload = Convert.ToBase64String(Encoding.UTF8.GetBytes(body));

                var sign = Cryptor.EncryptSHA1(playload, this.CookieName);
                var header = Convert.ToBase64String(Encoding.UTF8.GetBytes(this.CookieName));
                StringBuilder builder = new StringBuilder();
                builder.Append(header);
                builder.Append(".");
                builder.Append(playload);
                builder.Append(".");
                builder.Append(sign);
                return builder.ToString();

            });
        }
    }
    public class OpenAuthenticationHandler : AuthenticationHandler<OpenAuthenticationOptions>, IAuthenticationHandler, IAuthenticationSignInHandler, IAuthenticationSignOutHandler
    {
        public OpenAuthenticationHandler(IOptionsMonitor<OpenAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder)
            : base(options, logger, encoder, new SystemClock()) { }



        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //尝试从客户端得到Ticket
            var result = EnsureCookieTicket();
            //if (!result.Succeeded)
            //{
            //    return result;
            //}
            //服务器端验证是否过期
            //var context = new CookieValidatePrincipalContext(Context, Scheme, null, result.Ticket);
            //if (context.Principal == null)
            //{
            //    return AuthenticateResult.Fail("No principal.");
            //}

            return result;
        }

        private async Task<AuthenticateResult> ReadCookieTicket()
        {
            var token = string.Empty;
            if (this.Context.Request.Cookies.ContainsKey(this.Options.CookieName))
            {
                token = this.Context.Request.Cookies[this.Options.CookieName];
            }
            else if (this.Context.Request.Query.ContainsKey(this.Options.QueryName))
            {
                token = this.Context.Request.Query[this.Options.QueryName];
            }
            else
            {
                token = this.Context.Request.Headers[this.Options.TokenName].FirstOrDefault();
            }

            if (string.IsNullOrEmpty(token))
            {
                return AuthenticateResult.NoResult();
            }

            var ticket = await Options.TicketUnprotect(token);
            if (ticket == null)
            {
                return AuthenticateResult.Fail("Unprotect ticket failed");
            }
            var currentUtc = Clock.UtcNow;
            var issuedUtc = ticket.Properties.IssuedUtc;
            var expiresUtc = ticket.Properties.ExpiresUtc;

            if (expiresUtc != null && expiresUtc.Value < currentUtc)
            {
                return AuthenticateResult.Fail("Ticket expired");
            }
            return AuthenticateResult.Success(ticket);
        }
        private Task<AuthenticateResult> _readCookieTask;
        private Task<AuthenticateResult> EnsureCookieTicket()
        {
            // We only need to read the ticket once
            if (_readCookieTask == null)
            {
                _readCookieTask = ReadCookieTicket();
            }
            return _readCookieTask;
        }
        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            var redirectUri = properties.RedirectUri;
            if (string.IsNullOrEmpty(redirectUri))
            {
                redirectUri = OriginalPathBase + Request.Path + Request.QueryString;
            }
            var loginUri = Options.LoginPath + QueryString.Create(Options.ReturnUrlParameter, redirectUri);
            this.Response.Redirect(loginUri);
            return Task.CompletedTask;
            //return base.HandleChallengeAsync(properties);
        }
        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            var returnUrl = properties.RedirectUri;
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = OriginalPathBase + Request.Path + Request.QueryString;
            }
            var accessDeniedUri = Options.AccessDeniedPath + QueryString.Create(Options.ReturnUrlParameter, returnUrl);
            this.Response.Redirect(accessDeniedUri);
            return Task.CompletedTask;
            //return base.HandleForbiddenAsync(properties);
        }
        public Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
        {
            return Task.Run(() =>
            {
                var token = this.Options.TicketProtect(user).Result;
                var options = new CookieOptions()
                {
                    Expires = properties.ExpiresUtc,
                    HttpOnly = true,
                };
                this.Context.Response.Cookies.Append(this.Options.CookieName, token, options);
                if (string.IsNullOrEmpty(properties.RedirectUri))
                {
                    this.Context.Response.Redirect("/");
                }
                else
                {
                    this.Context.Response.Redirect(properties.RedirectUri);
                }
            });
        }
        public Task SignOutAsync(AuthenticationProperties properties)
        {
            return Task.Run(() =>
            {
                this.Context.Response.Cookies.Delete(this.Options.CookieName);
                this.Context.Response.Redirect("/");
            });
        }

    }
}