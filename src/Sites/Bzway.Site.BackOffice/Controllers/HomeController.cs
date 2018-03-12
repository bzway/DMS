using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bzway.Module.Wechat.Interface;
using Microsoft.Extensions.Logging;
using System.Threading;
using Bzway.Framework.Application;
using Microsoft.AspNetCore.Authorization;

namespace Bzway.Sites.BackOffice.Controllers
{

    public class HomeController : BaseController<HomeController>
    {
        #region ctor
        //readonly IWechatService wechatService;
        readonly ITenant tenant;
        public HomeController(
            //IWechatService wechatService,
            ITenant tenant,
            ISiteService siteService,
            ILoggerFactory loggerFactory) : base(loggerFactory, siteService)
        {
            //this.wechatService = wechatService;
            this.tenant = tenant;
        }
        #endregion
        public IActionResult Index()
        {
            var a = this.tenant.Site.Name;
            return View();
        }
    }
}
