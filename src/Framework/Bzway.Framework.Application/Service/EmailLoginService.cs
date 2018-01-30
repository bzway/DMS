using Bzway.Database.Core;
using System;
using System.Collections.Generic;
using Bzway.Framework.Application.Entity;
using Microsoft.Extensions.Logging;
using Bzway.Common.Utility;
using System.Linq;

namespace Bzway.Framework.Application
{
    internal class AppLoginService : ILoginService
    {
        #region ctor
        private readonly ILogger<AppLoginService> logger;
        private readonly ISystemDatabase db;
        public AppLoginService(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger<AppLoginService>();
            this.db = SystemDatabase.GetDatabase();
        }

        #endregion

        public Result<UserProfile> Login(string userName, string password, string validateCode)
        {
            throw new NotImplementedException();
        }
    }
}