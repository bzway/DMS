using Bzway.Database.Core;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Bzway.Framework.Application;
using Bzway.Framework.Content.Entity;
using System.Security.Principal;
using System.Linq;

namespace Bzway.Framework.Content
{
    /// <summary>
    /// GrantRequest service
    /// </summary>
    public partial class WebContentService : BaseService<WebContentService>, IWebContentService
    {

        #region ctor
        public WebContentService(ILoggerFactory loggerFactory, ITenant tenant, IPrincipal user) : base(loggerFactory, tenant, user)
        { }
        #endregion
        public WebPage FindPage(string PageUrl)
        {
            return this.db.Entity<WebPage>().Query().Where(m => m.AppKey == PageUrl).FirstOrDefault();
        }
    }
}