using Bzway.Database.Core;
namespace Bzway.Framework.Application.Entity
{
    public class Site : EntityBase
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Domains { get; set; }
        public string TimeZone { get; set; }
        public string ProviderName { get; set; }
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
        public string StaticFilePrefix { get; set; }
    }
}