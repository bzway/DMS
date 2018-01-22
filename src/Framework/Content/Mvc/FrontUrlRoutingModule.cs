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
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web; 
using System.Web.Mvc;

namespace Bzway.Mvc
{
    public class FrontUrlRoutingModule : UrlRoutingModule
    {
        protected override void Init(HttpApplication application)
        {
            application.PostResolveRequestCache += new EventHandler(application_PostResolveRequestCache);
        }

        void application_PostResolveRequestCache(object sender, EventArgs e)
        {
            HttpContextBase context = new FrontHttpContextWrapper(((HttpApplication)sender).Context);
            this.PostResolveRequestCache(context);
            //if (Site.Current != null)
            //{
            //    UrlRedirect(context);
            //}
        }
    }
}
