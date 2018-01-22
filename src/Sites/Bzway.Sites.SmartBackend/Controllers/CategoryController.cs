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

namespace Bzway.Sites.SmartBackend.Controllers
{
    public class CategoryController : ApiController<Category, CategoryController>
    {
        #region ctor
        public CategoryController(ILoggerFactory loggerFactory,
            ITenant tenant) : base(loggerFactory, tenant)
        {
        }
        #endregion
        [HttpGet]
        public IActionResult Index()
        {
            var query = this.db.Query();
            var list = query.ToList();
            return View(list);
        }
        [HttpGet]
        public IActionResult Detail(string id)
        {
            var model = this.db.Query().FirstOrDefault(m => m.Id == id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            UserManager userManager = AppEngine.GetService<UserManager>();
            var status = userManager.PasswordSignIn(model.UserName, model.Password, true);
            ModelState.AddModelError("", "LoginModelRequired");
            return View();
        }
    }
}