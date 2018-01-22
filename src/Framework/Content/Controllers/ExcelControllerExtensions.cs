using Microsoft.Owin;
using Owin;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace OpenData.Framework.Common.Core
{
    public static class ExcelControllerExtensions
    {
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="content"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static ActionResult Excel(this Controller controller, object model, String fileName)
        {
            controller.ViewData.Model = model;
            var result = new ExcelResult(fileName)
            {
                ViewName = null,
                MasterName = null,
                ViewData = controller.ViewData,
                TempData = controller.TempData,
                ViewEngineCollection = controller.ViewEngineCollection
            };
            return result;
        }
    }
}