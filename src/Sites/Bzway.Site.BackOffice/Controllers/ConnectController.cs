using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Bzway.Sites.BackOffice.Models;
using Microsoft.Extensions.Logging;
using Bzway.Framework.Application;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Bzway.Sites.BackOffice.Controllers
{

    [Route("Connect")]
    public class ConnectController : BaseController<ConnectController>
    {
        readonly ILoginProvider loginProvider;
        public ConnectController(ILoginProvider loginProvider, ILoggerFactory loggerFactory, ISiteService siteService) : base(loggerFactory, siteService)
        {
            this.loginProvider = loginProvider;
        }
        [HttpPost]
        [Route("Authorize")]
        [AllowAnonymous]
        public Result<AuthorizationResponseModel> Authorize(AuthorizationRequestModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return Result<AuthorizationResponseModel>.Fail(ResultCode.Error);

            }
            var provider = this.loginProvider.TryResolveService(model.GrantType);
            if (provider == null)
            {
                return Result<AuthorizationResponseModel>.Fail(ResultCode.Error);
            }
            var result = provider.Login(model.AppId, model.Signature,model.Random);

            if (result.Code == ResultCode.OK)
            {
                var token = this.HttpContext.Principal().SaveToken(new UserIdentity()
                {
                    Id = result.Data.Id,
                    Language = result.Data.Language,
                     
                    Name = result.Data.Name,
                    Roles = string.Join(',', result.Data.Roles),
                    //Version = result.Data.Birthday,
                }, 100);
                return Result<AuthorizationResponseModel>.Success(token.Data);
            }

            return Result<AuthorizationResponseModel>.Fail(ResultCode.Error);

        }

        [HttpGet]
        [Route("RefreshToken")]

        public Result<AuthorizationResponseModel> RefreshToken(string refreshToken)
        {
            return Result<AuthorizationResponseModel>.Success();
        }
    }
}