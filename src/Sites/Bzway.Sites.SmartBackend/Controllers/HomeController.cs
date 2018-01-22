using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bzway.Sites.SmartBackend.Models;
using Bzway.Common.Script;
using Bzway.Common.Share;
using Bzway.Framework.Application;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace Bzway.Sites.SmartBackend.Controllers
{
    [Authorize]
    public class HomeController : BaseController<HomeController>
    {
        #region ctor
        public HomeController(
            ILoggerFactory loggerFactory,
            ITenant tenant) : base(loggerFactory, tenant)
        {

        }
        #endregion
        public IActionResult Index(string PageUrl)
        {
            return View();
        }
    }
}