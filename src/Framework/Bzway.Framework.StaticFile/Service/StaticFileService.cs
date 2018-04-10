using Bzway.Database.Core;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Bzway.Framework.Application;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System.Security.Principal;
using Microsoft.Extensions.Configuration;

namespace Bzway.Framework.StaticFile
{
    /// <summary>
    /// GrantRequest service
    /// </summary>
    public partial class StaticFileService : BaseService<StaticFileService>, IStaticFileService
    {

        #region ctor
        readonly ISiteService siteService;
        readonly IFileProvider fileProvider;
        public StaticFileService(ISiteService siteService, ILoggerFactory loggerFactory, ITenant tenant, IPrincipal user) : base(loggerFactory, tenant, user)
        {
            this.siteService = siteService;
            var site = this.tenant.Site;
            var root = Path.Combine(Directory.GetCurrentDirectory(), AppEngine.Default.DataFolder, "sites", site.Name, "static");
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            this.fileProvider = new PhysicalFileProvider(root);
        }
        #endregion


        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return this.fileProvider.GetDirectoryContents(subpath);
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            return this.fileProvider.GetFileInfo(subpath);
        }

        public IChangeToken Watch(string filter)
        {
            return this.fileProvider.Watch(filter);
        }
    }
}