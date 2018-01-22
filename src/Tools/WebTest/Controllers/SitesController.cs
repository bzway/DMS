//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using OpenData.Business.Model;
//using WebTest.Models;

//namespace WebTest.Controllers
//{
//    public class SitesController : Controller
//    {
//        private MyClass db = new MyClass();

//        // GET: Sites
//        public ActionResult Index()
//        {
//            return View(db.Sites.ToList());
//        }

//        // GET: Sites/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Site site = db.Sites.Find(id);
//            if (site == null)
//            {
//                return HttpNotFound();
//            }
//            return View(site);
//        }

//        // GET: Sites/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: Sites/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "ID,Name,Caption,LogoUrl,FaviconUrl,Description,Domains,TimeZone,IsShowLogo,IsShowMenu,IsShowSideBar,ProviderName,DatabaseName,ConnectionString")] Site site)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Sites.Add(site);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(site);
//        }

//        // GET: Sites/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Site site = db.Sites.Find(id);
//            if (site == null)
//            {
//                return HttpNotFound();
//            }
//            return View(site);
//        }

//        // POST: Sites/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "ID,Name,Caption,LogoUrl,FaviconUrl,Description,Domains,TimeZone,IsShowLogo,IsShowMenu,IsShowSideBar,ProviderName,DatabaseName,ConnectionString")] Site site)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(site).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(site);
//        }

//        // GET: Sites/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Site site = db.Sites.Find(id);
//            if (site == null)
//            {
//                return HttpNotFound();
//            }
//            return View(site);
//        }

//        // POST: Sites/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            Site site = db.Sites.Find(id);
//            db.Sites.Remove(site);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
