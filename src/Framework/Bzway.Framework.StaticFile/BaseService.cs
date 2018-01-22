using Bzway.Data.Core;
using Bzway.Framework.Application;
using Microsoft.Extensions.Logging;
using System.Security.Principal;

namespace Bzway.Framework.StaticFile
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
            this.db = Bzway.Data.Core.OpenDatabase.GetDatabase(this.tenant.Site.ProviderName, this.tenant.Site.ConnectionString, this.tenant.Site.DatabaseName);
            this.user = user;
        }
        #endregion
    }
}