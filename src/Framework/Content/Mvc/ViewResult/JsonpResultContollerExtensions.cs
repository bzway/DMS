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
 
   //extension methods for the controller to allow jsonp.
    public static class JsonpResultContollerExtensions
    {
        public static JsonpResult Jsonp(this Controller controller, object data)
        {
            JsonpResult result = new JsonpResult();
            result.Data = data;
            result.ExecuteResult(controller.ControllerContext);
            return result;
        }
    }
}
