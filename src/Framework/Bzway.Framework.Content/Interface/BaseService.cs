using Bzway.Database.Core;
using Bzway.Framework.Application;
using Microsoft.Extensions.Logging;
using System.Security.Principal;

namespace Bzway.Framework.Content
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
            var site = this.tenant.Site;
            this.db = SystemDatabase.GetDatabase(site.ProviderName, site.ConnectionString, site.DatabaseName);
            this.user = user;
        }
        #endregion
    }
}