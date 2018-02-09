using Bzway.Framework.Application.Entity;
using System.Collections.Generic;
using System;

namespace Bzway.Framework.Application
{
    public interface ITokenService
    {
        Result<TokenModel> GenerateAccessToken(string AppId, string ScretKey, string Code);

        Result<string> GenerateRequestCode(string AppId, string UserId);
    }
}
