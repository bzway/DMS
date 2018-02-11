using Bzway.Framework.Application.Entity;
using System.Collections.Generic;
using System;
using System.Security.Claims;

namespace Bzway.Framework.Application
{
    public interface ITokenService
    {
        Result<string> GenerateRequestCode(string AppId, string UserId);
        Result<TokenModel> GenerateAuthAccessToken(string AppId, string secretKey, string Code);
        Result<TokenModel> GenerateClientAccessToken(string AppId, string secretKey);
        Result<TokenModel> GenerateUserAccessToken(ClaimsIdentity identity);
        Result<ClaimsIdentity> GetUserToken(string token);
    }
}
