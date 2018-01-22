//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using Bzway.Site.BackOffice;
//using Microsoft.Extensions.Logging;
//using Bzway.Module.Wechat.Interface;
//using Bzway.Data.Core;

//public class testsController : Controller
//{
//    #region ctor

//    private readonly IWechatService wechatService;
//    protected readonly ILogger logger;

//    public testsController( ILoggerFactory loggerFactory,
//        IWechatService wechatService)
//    {
//        this.logger = loggerFactory.CreateLogger<testsController>();
//        this.wechatService = wechatService;
//    }
//    #endregion

//    // GET: tests
//    public async Task<IActionResult> Index()
//    {
//        return View(await db.test.ToListAsync());
//    }

//    // GET: tests/Details/5
//    public async Task<IActionResult> Details(int? id)
//    {
//        if (id == null)
//        {
//            return NotFound();
//        }

//        var test = await db.test.SingleOrDefaultAsync(m => m.Id == id);
//        if (test == null)
//        {
//            return NotFound();
//        }

//        return View(test);
//    }

//    // GET: tests/Create
//    public IActionResult Create()
//    {
//        return View();
//    }

//    // POST: tests/Create
//    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> Create([Bind("Id,Age,Name")] test test)
//    {
//        if (ModelState.IsValid)
//        {
//            db.Add(test);
//            await db.SaveChangesAsync();
//            return RedirectToAction("Index");
//        }
//        return View(test);
//    }

//    // GET: tests/Edit/5
//    public async Task<IActionResult> Edit(int? id)
//    {
//        if (id == null)
//        {
//            return NotFound();
//        }

//        var test = await db.test.SingleOrDefaultAsync(m => m.Id == id);
//        if (test == null)
//        {
//            return NotFound();
//        }
//        return View(test);
//    }

//    // POST: tests/Edit/5
//    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> Edit(int id, [Bind("Id,Age,Name")] test test)
//    {
//        if (id != test.Id)
//        {
//            return NotFound();
//        }

//        if (ModelState.IsValid)
//        {
//            try
//            {
//                db.Update(test);
//                await db.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!testExists(test.Id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }
//            return RedirectToAction("Index");
//        }
//        return View(test);
//    }

//    // GET: tests/Delete/5
//    public async Task<IActionResult> Delete(int? id)
//    {
//        if (id == null)
//        {
//            return NotFound();
//        }

//        var test = await db.test.SingleOrDefaultAsync(m => m.Id == id);
//        if (test == null)
//        {
//            return NotFound();
//        }

//        return View(test);
//    }

//    // POST: tests/Delete/5
//    [HttpPost, ActionName("Delete")]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> DeleteConfirmed(int id)
//    {
//        var test = await db.test.SingleOrDefaultAsync(m => m.Id == id);
//        db.test.Remove(test);
//        await db.SaveChangesAsync();
//        return RedirectToAction("Index");
//    }

//    private bool testExists(int id)
//    {
//        return db.test.Any(e => e.Id == id);
//    }
//}

