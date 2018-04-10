using Bzway.Framework.Application.Entity;
using System.Collections.Generic;
using System;
using System.Security.Claims;

namespace Bzway.Framework.Application
{
    public interface ITokenService
    {
        Result<string> GenerateRequestCode(string AppId, string UserId);
        /// <summary>
        /// 第三方应用请求用户授权凭据
        /// </summary>
        /// <param name="AppId"></param>
        /// <param name="secretKey"></param>
        /// <param name="Code">用户请求码，只能使用一次，5分钟未被使用自动过期</param>
        /// <returns></returns>
        Result<TokenModel> GenerateAuthAccessToken(string AppId, string secretKey, string Code);
        /// <summary>
        /// 客户端凭证
        /// </summary>
        /// <param name="AppId"></param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        Result<TokenModel> GenerateClientAccessToken(string AppId, string secretKey);
        Result<TokenModel> GenerateUserAccessToken(ClaimsIdentity identity);
        Result<ClaimsIdentity> GetUserToken(string token);
    }
}
