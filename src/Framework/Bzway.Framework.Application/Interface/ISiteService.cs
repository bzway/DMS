using Bzway.Framework.Application.Entity;
using System.Collections.Generic;

namespace Bzway.Framework.Application
{
    public interface ISiteService
    {
        Site FindSiteByID(string siteID);
        Site FindSiteByName(string siteName);
        IEnumerable<Site> FindUserSites();
        void CreateOrUpdateSite(Site site);
        void DeleteSiteByID(string siteID);
    }
}
