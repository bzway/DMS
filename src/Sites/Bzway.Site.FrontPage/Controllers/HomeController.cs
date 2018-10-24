using Bzway.Common.Script;
using Bzway.Common.Share;
using Bzway.Framework.Application;
using Bzway.Framework.Application.Entity;
using Bzway.Framework.Content;
using Bzway.Framework.DistributedFileSystemClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System;
using Bzway.Data.Core;

namespace Bzway.Sites.FrontPage.Controllers
{
    public class HomeController : BaseController<HomeController>
    {
        #region ctor
        readonly IWebContentService webContentService;
        readonly IDistributedFileSystemService staticFileService;
        public HomeController(
            IDistributedFileSystemService staticFileService,
            IWebContentService webContentService,
            ILoggerFactory loggerFactory,
            ITenant tenant) : base(loggerFactory, tenant)
        {
            this.webContentService = webContentService;
            this.staticFileService = staticFileService;
        }
        #endregion
        public IActionResult Index(string PageUrl)
        {

            var site = this.tenant.Site;
            //静态文件
            var result = StaticFile(site, PageUrl);
            if (result != null)
            {
                return result;
            }
            //动态文件 - 通过动态调用用户创建的dll生成html           
            result = ReflectedPage(site, PageUrl);
            if (result != null)
            {
                return result;
            }
            //动态内容
            result = DynamicContent(site, PageUrl);
            if (result != null)
            {
                return result;
            }
            //反向代理
            result = ProxyPage(site, PageUrl);
            if (result != null)
            {
                return result;
            }

            return View();
        }

        IActionResult StaticFile(Site site, string PageUrl)
        {
            var path = PageUrl;
            if (!string.IsNullOrEmpty(site.StaticFilePrefix) && PageUrl.StartsWith(site.StaticFilePrefix, System.StringComparison.CurrentCultureIgnoreCase))
            {
                path = PageUrl.Remove(0, site.StaticFilePrefix.Length);
            }
            var fileInfo = this.staticFileService.GetFileStream(path);
            if (fileInfo == null)
            {
                return NotFound();
            }
            var contentType = fileInfo.Info.ContentType;
            if (string.IsNullOrEmpty(contentType))
            {
                if (!this.ContentTypeProvider.TryGetContentType(path, out contentType))
                {
                    contentType = "application/octet-stream";
                }
            }
            return File(fileInfo.Stream.OutputStream, contentType);
        }
        IActionResult ProxyPage(Site site, string PageUrl)
        {
            //WebClient client = new WebClient();

            //var path = this.Request.Path.Value;
            //var contentType = string.Empty;
            //foreach (var item in this.Request.Headers["Accept"])
            //{
            //    if (item.Contains("html"))
            //    {
            //        contentType = "html";
            //        break;
            //    }
            //}
            //var file = Path.Combine(Directory.GetCurrentDirectory() + site.Name + "/temp/" + this.Request.QueryString.GetHashCode().ToString() + "." + contentType);
            //FileInfo fileInfo = new FileInfo(file);
            //if (!fileInfo.Exists)
            //{
            //    if (!fileInfo.Directory.Exists)
            //    {
            //        fileInfo.Directory.Create();
            //    }
            //    using (var stream = fileInfo.OpenWrite())
            //    {
            //        //client.OpenRead("http://cn.bing.com/" + this.Request.Path.Value + this.Request.QueryString.Value).CopyTo(stream);
            //    }
            //}
            //using (var stream = fileInfo.OpenRead())
            //{
            //    stream.CopyTo(this.Response.Body);
            //}
            return null;
        }
        IActionResult DynamicContent(Site site, string PageUrl)
        {
            var build = site.GetMaper();
            var action = build.Action(PageUrl, site.Name);
            if (action == null)
            {
                return null;
            }
            var data = action.MapData.ContainsKey("data") ? action.MapData["data"].ToString() : string.Empty;
            var where = action.MapData.ContainsKey("where") ? action.MapData["where"].ToString() : string.Empty;
            var pageIndex = action.MapData.ContainsKey("pageIndex") ? action.MapData["pageIndex"].ToString() : string.Empty;
            var pageSize = action.MapData.ContainsKey("pageSize") ? action.MapData["pageSize"].ToString() : string.Empty;
            action.MapData.Add("model", GetData(site, data, where, pageIndex, pageSize));
            Dictionary<string, string> query = new Dictionary<string, string>();
            foreach (var item in this.Request.Query)
            {
                query.Add(item.Key, item.Value);
            }
            action.MapData.Add("query", query);
            var content = action.Render().Result;
            return Content(content, "text/html");
        }

        private object GetData(Site site, string data, string where, string pageIndex, string pageSize)
        {
            return new { age = 12, site };
        }

        IActionResult ReflectedPage(Site site, string PageUrl)
        {
            var page = this.webContentService.FindPage(PageUrl);
            if (page != null)
            {
                return Content(page.Name);
            }
            return null;
        }
    }
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
    public class DevelopmentController : HomeController
    {
        public DevelopmentController(
            IDistributedFileSystemService staticFileService,
            IWebContentService webContentService,
            ILoggerFactory loggerFactory,
            ITenant tenant) : base(staticFileService, webContentService, loggerFactory, tenant)
        { }
    }
    public class StagingController : HomeController
    {
        public StagingController(
            IDistributedFileSystemService staticFileService,
            IWebContentService webContentService,
            ILoggerFactory loggerFactory,
            ITenant tenant) : base(staticFileService, webContentService, loggerFactory, tenant)
        { }
    }
}
