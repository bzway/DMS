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
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Bzway.Common.Share.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Bzway.Sites.BackOffice.Controllers
{
    public class FileManagerController : BaseController<FileManagerController>
    {
        #region ctor
        public FileManagerController(ITenant tenant, ILoggerFactory loggerFactory) : base(loggerFactory, tenant) { }
        #endregion

        public IActionResult Index()
        {
            return View();
        }
    }
}