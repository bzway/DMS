using Bzway.Database.Core;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Bzway.Framework.Application.Entity;
using System.Security.Principal;
using System.Linq;

namespace Bzway.Framework.Application
{
    /// <summary>
    /// GrantRequest service
    /// </summary>
    public partial class SiteService : ISiteService
    {

        #region ctor
        protected readonly ILogger logger;
        protected readonly ISystemDatabase db;
        protected readonly IPrincipal user;
        public SiteService(ILoggerFactory loggerFactory, ITenant tenant, IPrincipal user)
        {
            this.logger = loggerFactory.CreateLogger<SiteService>();
            this.db = SystemDatabase.GetDatabase();
            this.user = user;
        }
        #endregion


        public IEnumerable<Site> FindUserSites()
        {
            string userID = this.user.ToString();
            List<Site> list = new List<Site>();
            foreach (var item in db.Entity<UserSite>().Query().Where(m => m.UserID == userID).ToList())
            {
                list.AddRange(db.Entity<Site>().Query().Where(m => m.Id == item.SiteID).ToList());
            }
            return list;
        }
        public Site FindSiteByID(string id)
        {
            return this.db.Entity<Site>().Query().Where(m => m.Id == id).First();
        }
        public Site FindSiteByName(string siteName)
        {
            return this.db.Entity<Site>().Query().Where(m => m.Name == siteName).First();
        }
        public void CreateOrUpdateSite(Site site)
        {
            if (!string.IsNullOrEmpty(site.Id))
            {
                this.db.Entity<Site>().Update(site);
                return;
            }
            site.Id = Guid.NewGuid().ToString("N");
            this.db.Entity<Site>().Insert(site);
            this.db.Entity<UserSite>().Insert(new UserSite() { UserID = this.user.ToString(), SiteID = site.Id });
        }
        public void DeleteSiteByID(string siteID)
        {
            this.db.Entity<Site>().Delete(siteID);
            var userSite = this.db.Entity<UserSite>().Query()
                .Where(m => m.SiteID == siteID)
                .First();
            this.db.Entity<UserSite>().Delete(userSite);
        }
    }
}