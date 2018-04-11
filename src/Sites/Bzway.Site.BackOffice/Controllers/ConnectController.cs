using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bzway.Framework.Application;
using Microsoft.AspNetCore.Authorization;
using Bzway.Sites.BackOffice.Models;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Bzway.Sites.BackOffice.Controllers
{
    [Route("Connect")]
    public class ConnectController : BaseController<ConnectController>
    {
        private readonly ILoginService loginService;
        private readonly ITokenService tokenService;
        public ConnectController(ILoginService loginService, ITokenService tokenService,
            ITenant tenant,
           ILoggerFactory loggerFactory) : base(loggerFactory, tenant)
        {
            this.loginService = loginService;
            this.tokenService = tokenService;
        }

        [HttpGet("Authorize")]
        [AllowAnonymous]
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
            if (model.Scope == "")
            {
                //var code = Guid.NewGuid().ToString("N");
            }
            var code = this.loginService.ToString();
            return Redirect(model.CallBack + "&" + model.ResponseType + "=" + code + "&state=" + model.State);
        }

        [HttpGet("AccessToken")]
        [AllowAnonymous]
        public ActionResult AccessToken(string appId, string secretKey, string code, string grantType)
        {
            if (string.IsNullOrEmpty(grantType))
            {
                grantType = "client_credential";
            }

            if (grantType == "client_credential")
            {
                var result = this.tokenService.GenerateClientAccessToken(appId, secretKey);
                return Redirect("/");
            }
            else
            {
                var result = this.tokenService.GenerateAuthAccessToken(appId, secretKey, code);
                return Redirect("/");
            }

        }
        [HttpGet("RefreshToken")]
        [AllowAnonymous]
        public ActionResult RefreshToken(string appId, string refreshToken, string grantType)
        {
            var result = this.tokenService.GenerateAuthAccessToken(appId, refreshToken, grantType);
            return Redirect("/");
        }
    }
}
