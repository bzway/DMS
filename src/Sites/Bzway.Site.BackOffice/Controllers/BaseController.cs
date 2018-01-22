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
        private void requestLog(string requestData)
        {
            this.stopwatch.Stop();
            var message = string.Format("ApiTracking:{0}:~/{1}/{2},from {3},at {4}, with {5}, takes {6} milliseconds", string.IsNullOrEmpty(this.method) ? "NULL" : this.method,   this.requestTime, requestData.ToString(), this.stopwatch.ElapsedMilliseconds);
            this.logger.LogInformation(message);
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }
    }

    [ApiTracking]
    public abstract class BaseController<T> : Controller
    {
        #region ctor
        protected ILogger logger;
        protected ISiteService siteService;
        public BaseController(ILoggerFactory loggerFactory, ISiteService siteService)
        {
            this.logger = loggerFactory.CreateLogger<T>();
            this.siteService = siteService;
        }
        #endregion

    }
}