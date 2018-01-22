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

namespace Bzway.Sites.SmartBackend.Controllers
{
    public class ArticleController : ApiController<Article, ArticleController>
    {
        #region ctor
        public ArticleController(
            ILoggerFactory loggerFactory,
            ITenant tenant) : base(loggerFactory, tenant)
        {

        }
        #endregion
        public IActionResult Index(string categroy, int? pageIndex, int? pageSize)
        {
            var query = this.db.Query();
            if (!string.IsNullOrEmpty(categroy))
            {
                query = query.Where(m => m.Categroy == categroy);
            }

            var list = query.Skip((pageIndex ?? 1 - 1) * (pageSize ?? 10)).Take(pageSize ?? 10).ToList();
            return View(list);
        }
    }
}