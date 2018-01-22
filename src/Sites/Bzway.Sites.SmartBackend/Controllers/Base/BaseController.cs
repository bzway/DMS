using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bzway.Sites.SmartBackend.Models;
using Bzway.Common.Script;
using Bzway.Common.Share;
using Bzway.Framework.Application;
using Bzway.Framework.Application.Entity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Net;

namespace Bzway.Sites.SmartBackend.Controllers
{
    public abstract class BaseController<T> : Controller
    {
        #region ctor
        protected readonly ITenant tenant;
        protected readonly ILogger logger;
        public BaseController(ILoggerFactory loggerFactory, ITenant tenant)
        {
            this.logger = loggerFactory.CreateLogger<T>();
            this.tenant = tenant;
        }
        #endregion

        protected FileExtensionContentTypeProvider ContentTypeProvider = new FileExtensionContentTypeProvider();
    }
}