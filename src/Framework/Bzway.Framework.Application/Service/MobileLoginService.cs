using Bzway.Database.Core;
using System;
using System.Collections.Generic;
using Bzway.Framework.Application.Entity;
using Microsoft.Extensions.Logging;
using Bzway.Common.Utility;
using System.Linq;

namespace Bzway.Framework.Application
{
    internal class AuthLoginService : ILoginService
    {
        #region ctor
        private readonly ILogger<AuthLoginService> logger;
        private readonly ISystemDatabase db;
        public AuthLoginService(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger<AuthLoginService>();
            this.db = SystemDatabase.GetDatabase();
        }


        #endregion

        public Result<UserProfile> Login(string userName, string password, string validateCode)
        {
            throw new NotImplementedException();
        }
 
    }
}