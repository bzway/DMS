using Bzway.Database.Core;
using Bzway.Framework.Application;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Principal;

namespace Bzway.Framework.StaticFile
{
    public abstract class BaseService<T>
    {
        #region ctor
        protected readonly ILogger<T> logger;
        protected readonly ITenant tenant;
        protected readonly ISystemDatabase db;
        protected readonly IPrincipal user;
        public BaseService(ILoggerFactory loggerFactory, ITenant tenant, IPrincipal user)
        {
            this.logger = loggerFactory.CreateLogger<T>();
            this.tenant = tenant;
            if (this.tenant.Site == null)
            {
                throw new Exception("No Site Found!");
            }
            this.db = SystemDatabase.GetDatabase(this.tenant.Site.ProviderName, this.tenant.Site.ConnectionString, this.tenant.Site.DatabaseName);
            this.user = user;
        }
        #endregion
    }
}