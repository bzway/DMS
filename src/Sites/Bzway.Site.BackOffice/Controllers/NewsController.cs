using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bzway.Sites.BackOffice;
using Microsoft.Extensions.Logging;
using Bzway.Module.Wechat.Interface;
using Bzway.Data.Core;
using Bzway.Module.Wechat.Entity;

namespace Bzway.Sites.BackOffice.Controllers
{

    public class NewsController : Controller
    {
        #region ctor

        private readonly IWechatService wechatService;
        protected readonly ILogger logger;

        public NewsController(ILoggerFactory loggerFactory,
            IWechatService wechatService)
        {
            this.logger = loggerFactory.CreateLogger<NewsController>();
            this.wechatService = wechatService;
            this.db = OpenDatabase.GetDatabase();
        }
        private readonly IDatabase db;

        #endregion

        // GET: tests
        public IActionResult Index()
        {
            return View(db.Entity<WechatNewsMaterial>().Query().ToList());
        }

        // GET: tests/Details/5
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = this.db.Entity<WechatNewsMaterial>().Query().Where(m => m.Id, id, CompareType.Equal).First();
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: tests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: tests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(WechatNewsMaterial model)
        {
            if (ModelState.IsValid)
            {
                this.db.Entity<WechatNewsMaterial>().Insert(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: tests/Edit/5
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = this.db.Entity<WechatNewsMaterial>().Query().Where(m => m.Id, id, CompareType.Equal).First();
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // POST: tests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, WechatNewsMaterial model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this.db.Entity<WechatNewsMaterial>().Update(model);
                }
                catch
                {
                    return View(model);
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: tests/Delete/5
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = this.db.Entity<WechatNewsMaterial>().Query().Where(m => m.Id, id, CompareType.Equal).First();
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }



        // POST: tests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            this.db.Entity<WechatNewsMaterial>().Delete(id);
            return RedirectToAction("Index");
        }
    }
}