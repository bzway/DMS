#region License
// 
// Copyright (c) 2013, Bzway team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace OpenData.Web.Mvc
{

    public class NestedContainerViewEngine : WebFormViewEngine
    {
        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            var result = base.FindView(controllerContext, viewName, masterName, useCache);

            return CreateNestedView(result, controllerContext);
        }

        public override ViewEngineResult FindPartialView(
            ControllerContext controllerContext,
            string partialViewName, bool useCache)
        {
            var result = base.FindPartialView(controllerContext, partialViewName, useCache);

            return CreateNestedView(result, controllerContext);
        }
        private ViewEngineResult CreateNestedView(ViewEngineResult result, ControllerContext controllerContext)
        {
            if (result.View == null)
            {
                return result;
            }

            var webFormView = (WebFormView)result.View;

            var wrappedView = new WrappedView(webFormView);

            var newResult = new ViewEngineResult(wrappedView, this);

            return newResult;
        }
    }
}