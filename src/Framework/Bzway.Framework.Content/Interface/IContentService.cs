using Bzway.Data.Core;
using Bzway.Framework.Application;
using Microsoft.Extensions.Logging;
using System.Security.Principal;

namespace Bzway.Framework.Content
{
    public interface IContentService
    {
    }
    public class ContentService : BaseService<ContentService>, IContentService
    {
        #region ctor
        public ContentService(ILoggerFactory loggerFactory, ITenant tenant, IPrincipal user) : base(loggerFactory, tenant, user) { }
        #endregion
    }
}