using Bzway.Data.Core;
using Bzway.Framework.Application;
using Microsoft.Extensions.Logging;
using System.Security.Principal;


namespace Bzway.Module.MemberClub
{
    public abstract class BaseService<T>
    {
        #region ctor
        protected readonly ILogger<T> logger;
        protected readonly ITenant tenant;
        protected readonly IDatabase db;
        protected readonly IPrincipal user;
        public BaseService(ILoggerFactory loggerFactory, ITenant tenant, IPrincipal user)
        {
            this.logger = loggerFactory.CreateLogger<T>();
            this.tenant = tenant;
            this.db = OpenDatabase.GetDatabase(tenant.Site.ProviderName, tenant.Site.ConnectionString, tenant.Site.DatabaseName);
            this.user = user;
        }
        #endregion
    }
}
