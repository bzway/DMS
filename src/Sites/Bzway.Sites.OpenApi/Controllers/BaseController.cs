using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Bzway.Framework.Application;
using Microsoft.Extensions;
using System.Diagnostics;

namespace Bzway.Sites.OpenApi.Controllers
{

    public abstract class BaseController<T> : Controller
    {
        #region ctor
        protected ILogger logger;
        protected ISiteService siteService;
        public BaseController(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger<T>();
        }
        #endregion

    }
}