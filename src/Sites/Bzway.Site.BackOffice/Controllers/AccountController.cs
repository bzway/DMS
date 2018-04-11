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
using Bzway.Sites.BackOffice.Models;

namespace Bzway.Sites.BackOffice.Controllers
{

    [Route("Account")]
    public class AccountController : BaseController<AccountController>
    {
        #region ctor
        private readonly ILoginService loginService;
        public AccountController(ILoginService loginService, ITenant tenant, ILoggerFactory loggerFactory) : base(loggerFactory, tenant)
        {
            this.loginService = loginService;
        }
        #endregion 
        public IActionResult LogOff(string code, string state, string url)
        {
            return this.SignOut(OpenAuthenticationOptions.DefaultSchemeName);
        }

        [HttpGet("Login")]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginRequestModel model, string returnUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var result = this.loginService.Login(model.UserName, model.Password, model.ValidateCode);
                if (result.Code != ResultCode.OK)
                {
                    ModelState.AddModelError("LoginModelRequired", result.Message);
                    return View(model);
                }

                ClaimsPrincipal principal = new ClaimsPrincipal();
                var identity = new ClaimsIdentity(OpenAuthenticationOptions.DefaultSchemeName);
                identity.AddClaim(new Claim(ClaimTypes.Name, result.Data.NickName));
                identity.AddClaim(new Claim("Language", result.Data.Language));
                foreach (var item in result.Data.Roles)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, item));
                }
                principal.AddIdentity(identity);

                if (string.IsNullOrEmpty(returnUrl))
                {
                    returnUrl = "/";
                }
                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    RedirectUri = returnUrl,
                    ExpiresUtc = DateTime.UtcNow.AddDays(30),
                };
                return this.SignIn(principal, properties, OpenAuthenticationOptions.DefaultSchemeName);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "AccountController.Login");
                ModelState.AddModelError("LoginModelRequired", "throw exception".ToLocalized());
                return View(model);
            }
        }
    }
}
