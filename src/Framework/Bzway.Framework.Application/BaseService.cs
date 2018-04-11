using Bzway.Framework.Application.Entity;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Security.Principal;
using System;
using Microsoft.AspNetCore.Http;
using Bzway.Framework.Application;
using Bzway.Database.Core;

namespace Bzway.Framework.Application
{
    public abstract class BaseService<T>
    {
        #region ctor
        protected readonly ILogger<T> logger;
        protected readonly ITenant tenant;
        protected readonly IPrincipal user;
        protected readonly ISystemDatabase db;
        public BaseService(ILoggerFactory loggerFactory, ITenant tenant)
        {
            this.logger = loggerFactory.CreateLogger<T>();
            this.tenant = tenant;
            this.db = this.tenant.MasterDatabase;
            this.user = this.tenant.Context.User;
        }
        #endregion
    }
}