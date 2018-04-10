using Bzway.Database.Core;
using System;
using System.Collections.Generic;
using Bzway.Framework.Application.Entity;
using Microsoft.Extensions.Logging;
using Bzway.Common.Utility;
using System.Linq;
using Bzway.Common.Share;
using System.Security.Claims;

namespace Bzway.Framework.Application
{
    internal class TokenService : ITokenService
    {
        #region ctor
        private readonly ILogger<TokenService> logger;
        private readonly ISystemDatabase db;
        private const string requestCodeKey = "request_code:";
        private const string authTokenKey = "access_token:auth_credential:";
        private const string clientTokenKey = "access_token:client_credential:";
        private const string userTokenKey = "access_token:user_credential:";
        public TokenService(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger<TokenService>();
            this.db = SystemDatabase.GetDatabase();
        }



        #endregion
        class requestCodeModel
        {
            public string AppId { get; set; }
            public string UserId { get; set; }
        }

        public Result<string> GenerateRequestCode(string AppId, string UserId)
        {
            var code = Guid.NewGuid().ToString("N");
            var key = requestCodeKey + code;
            var model = new requestCodeModel() { AppId = AppId, UserId = UserId };
            CacheManager.Default.RedisCacheProvider.Set(key, model, 10000);
            return Result<string>.Success(code);
        }
        public Result<TokenModel> GenerateAuthAccessToken(string appId, string secretKey, string code)
        {
            var key = requestCodeKey + code;
            var codeModel = CacheManager.Default.RedisCacheProvider.Get<requestCodeModel>(key);
            if (codeModel == null || !string.Equals(codeModel.AppId, appId))
            {
                return Result<TokenModel>.Fail(ResultCode.Error, "Wrong Request Code");
            }

            var client = this.db.Entity<Client>().Query().Where(m => m.AppKey == appId && m.AppSecret == secretKey).FirstOrDefault();
            if (client == null)
            {
                return Result<TokenModel>.Fail(ResultCode.Error, "Wrong AppId or SecretKey");
            }
          
            var accessToken = Guid.NewGuid().ToString("N");
            key = authTokenKey + accessToken;
            var model = new TokenModel()
            {
                ExpiredIn = 3600,
                RefreshToken = Guid.NewGuid().ToString("N"),
                Token = Guid.NewGuid().ToString("N"),
                UserId = codeModel.UserId,
            };
            CacheManager.Default.RedisCacheProvider.Set(key, model, model.ExpiredIn);
            return Result<TokenModel>.Success(model);
        }
        public Result<TokenModel> GenerateClientAccessToken(string appId, string secretKey)
        {
            var client = this.db.Entity<Client>().Query().Where(m => m.AppKey == appId && m.AppSecret == secretKey).FirstOrDefault();
            if (client == null)
            {
                return Result<TokenModel>.Fail(ResultCode.Error, "Wrong AppId or SecretKey");
            }
            var key = requestCodeKey + appId;
            //根据用户Id得到token key
            string tokenKey;
            if (CacheManager.Default.RedisCacheProvider.IsSet(key))
            {
                tokenKey = CacheManager.Default.RedisCacheProvider.Get<string>(key);
            }
            else
            {
                tokenKey = clientTokenKey + Guid.NewGuid().ToString("N");
                CacheManager.Default.RedisCacheProvider.Set(key, tokenKey);
            }

            TokenModel model = new TokenModel()
            {
                ExpiredIn = 3600,
                RefreshToken = Guid.NewGuid().ToString("N"),
                Token = Guid.NewGuid().ToString("N"),
            };
            //记录tokenKey
            CacheManager.Default.RedisCacheProvider.Set(tokenKey, model, model.ExpiredIn);
            return Result<TokenModel>.Success(model);
        }
        public Result<TokenModel> GenerateUserAccessToken(ClaimsIdentity identity)
        {
            var userKey = requestCodeKey + identity.Name;
            CacheManager.Default.RedisCacheProvider.Remove(userKey);
            var model = new TokenModel()
            {
                ExpiredIn = 6000,
                Token = Guid.NewGuid().ToString("N"),
            };
            var tokenKey = userTokenKey + model.Token;
            CacheManager.Default.RedisCacheProvider.Set(userKey, model.Token);
            //记录tokenKey
            CacheManager.Default.RedisCacheProvider.Set(tokenKey, identity, model.ExpiredIn);
            return Result<TokenModel>.Success(model);
        }
        public Result<ClaimsIdentity> GetUserToken(string token)
        {
            var key = userTokenKey + token;
            var model = CacheManager.Default.RedisCacheProvider.Get<ClaimsIdentity>(key);
            return Result<ClaimsIdentity>.Success(model);
        }
    }
}