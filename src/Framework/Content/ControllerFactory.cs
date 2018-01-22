#region License
// 
// Copyright (c) 2013, Bzway team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using OpenData.AppEngine;
using OpenData.AppEngine.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace OpenData.Web
{
    /// <summary>
    /// 
    /// </summary>
    [Dependency(typeof(IControllerFactory))]
    public class ControllerFactory : DefaultControllerFactory
    {
        /// <summary>
        /// Retrieves the controller instance for the specified request context and controller type.
        /// </summary>
        /// <param name="requestContext">The context of the HTTP request, which includes the HTTP context and route data.</param>
        /// <param name="controllerType">The type of the controller.</param>
        /// <returns>
        /// The controller instance.
        /// </returns>
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                return null;
            }
            return (IController)ApplicationEngine.Current.Resolve(controllerType);
        }
    }
}
