using Bzway.Database.Core;
namespace Bzway.Framework.Application.Entity
{
    public class UserSite : EntityBase
    {
        public string UserID { get; set; }
        public string SiteID { get; set; }
        public string Permission { get; set; }
        public bool IsAdmin { get; set; }
    }
}