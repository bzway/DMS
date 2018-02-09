using Bzway.Database.Core;
using System;
using System.Collections.Generic;
using Bzway.Framework.Application.Entity;
using Microsoft.Extensions.Logging;
using Bzway.Common.Utility;
using System.Linq;
using Bzway.Common.Share;

namespace Bzway.Framework.Application
{
    internal class TokenService : ITokenService
    {
        #region ctor
        private readonly ILogger<TokenService> logger;
        private readonly ISystemDatabase db;
        private const string reuestCodeKey = "request_code:";
        private const string authTokenKey = "access_token:auth_credential:";
        private const string clientTokenKey = "access_token:client_credential:";
        private const string userTokenKey = "access_token:user_credential:";
        public TokenService(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger<TokenService>();
            this.db = SystemDatabase.GetDatabase();
        }
        public Result<string> GenerateRequestCode(string AppId, string UserId)
        {
            var code = Guid.NewGuid().ToString("N");
            var key = reuestCodeKey + code;
            var model = new requestCodeModel() { AppId = AppId, UserId = UserId };
            CacheManager.Default.RedisCacheProvider.Set(key, model, 1000);
            return Result<string>.Success(code);
        }
        class requestCodeModel
        {
            public string AppId { get; set; }
            public string UserId { get; set; }
        }
        public Result<TokenModel> GenerateAccessToken(string AppId, string SecretKey, string Code)
        {
            var client = this.db.Entity<Client>().Query().Where(m => m.AppKey == AppId && m.AppSecret == SecretKey).FirstOrDefault();
            if (client == null)
            {
                return Result<TokenModel>.Fail(ResultCode.Error, "Wrong AppId or SecretKey");
            }
            var key = reuestCodeKey + Code;
            var model = CacheManager.Default.RedisCacheProvider.Get<requestCodeModel>(key);
            if (model == null || !string.Equals(model.AppId, AppId))
            {
                return Result<TokenModel>.Fail(ResultCode.Error, "Wrong Request Code");
            }
            var accessToken = Guid.NewGuid().ToString("N");
            key = authTokenKey + accessToken;
            return null;
        }



        #endregion


    }
}