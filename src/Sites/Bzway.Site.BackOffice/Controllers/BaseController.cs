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
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.Extensions.Options;

namespace Bzway.Sites.BackOffice.Controllers
{
    public class ApiTrackingAttribute : ActionFilterAttribute
    {
        private readonly Stopwatch stopwatch;
        private ILogger logger;

        private DateTime requestTime;
        private string path;
        private string method;
        private string ipAddress;
        public ApiTrackingAttribute()
        {
            this.stopwatch = new Stopwatch();
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ILoggerFactory loggerFactory = (ILoggerFactory)context.HttpContext.RequestServices.GetService(typeof(ILoggerFactory));
            this.logger = loggerFactory.CreateLogger<ApiTrackingAttribute>();
            this.stopwatch.Start();
            this.path = context.HttpContext.Request.Path;
            this.requestTime = DateTime.UtcNow;
            this.method = context.HttpContext.Request.Method;
            this.ipAddress = context.HttpContext.Request.Path;
            base.OnActionExecuting(context);
        }
        private void RequestLog(string requestData)
        {
            this.stopwatch.Stop();
            var message = $"ApiTracking:{this.method}:~/{this.path},from {this.ipAddress} at {this.requestTime} with {requestData}, takes {this.stopwatch.ElapsedMilliseconds} milliseconds";
            this.logger.LogInformation(message);
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {

            try
            {
                //获取结果
                IActionResult actionResult = context.Result;
                //判断结果是否是一个ViewResult
                if (actionResult is ViewResult)
                {
                    ViewResult viewResult = actionResult as ViewResult;
                    //下面的代码就是执行这个ViewResult，并把结果的html内容放到一个StringBuiler对象中
                    var services = context.HttpContext.RequestServices;
                    var executor = services.GetRequiredService<ViewResultExecutor>();
                    var option = services.GetRequiredService<IOptions<MvcViewOptions>>();
                    var result = executor.FindView(context, viewResult);
                    result.EnsureSuccessful(originalLocations: null);
                    var view = result.View;
                    StringBuilder builder = new StringBuilder();
                    using (var writer = new StringWriter(builder))
                    {
                        var viewContext = new ViewContext(
                            context,
                            view,
                            viewResult.ViewData,
                            viewResult.TempData,
                            writer,
                            option.Value.HtmlHelperOptions);

                        view.RenderAsync(viewContext).GetAwaiter().GetResult();
                        //这句一定要调用，否则内容就会是空的
                        writer.Flush();
                    }
                    //按照规则生成静态文件名称
                    string area = context.RouteData.Values["area"].ToString().ToLower();
                    string controllerName = context.RouteData.Values["controller"].ToString().ToLower();
                    string actionName = context.RouteData.Values["action"].ToString().ToLower();
                    string devicedir = Path.Combine(AppContext.BaseDirectory, "wwwroot", area);
                    if (!Directory.Exists(devicedir))
                    {
                        Directory.CreateDirectory(devicedir);
                    }

                    //写入文件
                    string filePath = Path.Combine(AppContext.BaseDirectory, "wwwroot", area, controllerName + "-" + actionName + ".html");
                    using (FileStream fs = File.Open(filePath, FileMode.Create))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                        {
                            sw.Write(builder.ToString());
                        }
                    }
                    //输出当前的结果
                    ContentResult contentresult = new ContentResult();
                    contentresult.Content = builder.ToString();
                    contentresult.ContentType = "text/html";
                    context.Result = contentresult;
                    this.RequestLog(contentresult.Content);
                }
            }
            catch { }

            base.OnActionExecuted(context);
        }
    }

    [ApiTracking]
    public abstract class BaseController<T> : Controller
    {
        #region ctor
        protected readonly ILogger logger;
        protected readonly ITenant tenant;
        public BaseController(ILoggerFactory loggerFactory, ITenant tenant)
        {
            this.logger = loggerFactory.CreateLogger<T>();
            this.tenant = tenant;
        }
        #endregion

    }
}