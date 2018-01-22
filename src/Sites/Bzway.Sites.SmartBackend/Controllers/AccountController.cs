using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bzway.Sites.SmartBackend.Models;
using Bzway.Common.Script;
using Bzway.Common.Share;
using Bzway.Framework.Application;
using Bzway.Framework.Application.Entity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Net;
using System.Security.Claims;

namespace Bzway.Sites.SmartBackend.Controllers
{
    public class AccountController : BaseController<AccountController>
    {
        #region ctor

        public AccountController(
            ILoggerFactory loggerFactory,
            ITenant tenant) : base(loggerFactory, tenant)
        { }
        #endregion
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = UserManager.PasswordSignIn(model.UserName, model.Password);
            if (result.Code == 0)
            {
                var identity = new ClaimsIdentity(Startup.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, model.UserName, ClaimValueTypes.String));
                foreach (var item in result.Data.Roles)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, item, ClaimValueTypes.String));
                }
                var user = new ClaimsPrincipal(identity);
                await this.HttpContext.Authentication.SignInAsync(Startup.AuthenticationScheme, user);
                if (string.IsNullOrEmpty(returnUrl))
                {
                    return this.Redirect("/");
                }
                return this.Redirect(returnUrl);
            }
            ModelState.AddModelError("", "登录失败");
            return View();
        }
    }
}