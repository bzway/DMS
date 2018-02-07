﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bzway.Framework.Application;
using Microsoft.AspNetCore.Authorization;
using Bzway.Sites.OpenApi.Models;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Bzway.Sites.OpenApi.Controllers
{
    [Route("Connect")]
    public class ConnectController : Controller
    {
        private readonly ITenant tenant;
        private readonly ILogger logger;
        private readonly ILoginProvider loginProvider;
        public ConnectController(ITenant tenant, ILoginProvider loginProvider, ILoggerFactory loggerFactory)
        {
            this.tenant = tenant;
            this.loginProvider = loginProvider;
            this.logger = loggerFactory.CreateLogger<ConnectController>();
        }

        [HttpGet("Authorize")]
        public ActionResult Authorize(string appId, string responseType, string scope, string state, string callBack)
        {
            if (string.IsNullOrEmpty(responseType))
            {
                responseType = "code";
            }
            if (string.IsNullOrEmpty(scope))
            {
                scope = "base_info";
            }
            if (!this.HttpContext.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("login", new
                {
                    returnUrl = $"/Connect/Authorize?appId={appId}&responseType={responseType}&scope={scope}&state={state}&callBack={callBack}"
                });
            }

            AuthorizationModel model = new AuthorizationModel()
            {
                AppId = appId,
                ResponseType = responseType,
                Scope = scope,
                CallBack = callBack,
                State = state,
            };
            if (scope == "base_info")
            {
                return Authorize(model);
            }
            return View(model);
        }

        [HttpPost("Authorize")]
        [ValidateAntiForgeryToken]
        public ActionResult Authorize(AuthorizationModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }
            var code = this.loginProvider.ToString();
            return Redirect(model.CallBack + "&" + model.ResponseType + "=" + code + "&state=" + model.State);
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
                var loginProvider = this.loginProvider.TryResolveService(model.GrantType);
                if (loginProvider == null)
                {
                    ModelState.AddModelError("LoginModelRequired", "GrantType is Wrong");
                    return View(model);
                }
                var result = loginProvider.Login(model.AppId, model.SecretKey, model.ValidateCode);
                if (result.Code != ResultCode.OK)
                {
                    ModelState.AddModelError("LoginModelRequired", result.Message);
                    return View(model);
                }
                this.User.AddIdentity(new UserIdentity()
                {
                    User = new UserModel()
                    {
                        Id = result.Data.Id,
                        NickName = result.Data.NickName,
                        Roles = string.Join(',', result.Data.Roles),
                        Version = 1,
                        Language = result.Data.Language,
                    }
                });



                if (string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect("/");
                }

                return Redirect(returnUrl);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "AccountController.Login");
                ModelState.AddModelError("LoginModelRequired", "抛出异常");
                return View(model);
            }
        }

        [HttpGet("AccessToken")]
        public ActionResult AccessToken(string appId, string secretKey, string code, string grantType)
        {
            var result = this.loginProvider.TryResolveService(grantType).Login(appId, secretKey, code);
            return Redirect("/");
        }
        [HttpGet("RefreshToken")]
        public ActionResult RefreshToken(string appId, string refreshToken, string grantType)
        {
            var result = this.loginProvider.TryResolveService(grantType).Login(appId, refreshToken, grantType);
            return Redirect("/");
        }
    }
}
