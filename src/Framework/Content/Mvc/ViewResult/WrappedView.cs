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
    public class WrappedView : IView, IDisposable
    {
        private bool _disposed;

        public WrappedView(WebFormView baseView)
        {
            BaseView = baseView;
        }

        public WebFormView BaseView { get; private set; }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            _disposed = true;
        }
        public void Render(ViewContext viewContext, TextWriter writer)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException("viewContext");
            }

            object viewInstance = BuildManager.CreateInstanceFromVirtualPath(BaseView.ViewPath, typeof(object));
            if (viewInstance == null)
            {
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.CurrentUICulture,
                        "The view found at '{0}' was not created.",
                        BaseView.ViewPath));
            }



            var viewPage = viewInstance as ViewPage;
            if (viewPage != null)
            {
                //RenderViewPage(viewContext, viewPage);
                return;
            }

            ViewUserControl viewUserControl = viewInstance as ViewUserControl;
            if (viewUserControl != null)
            {
                //RenderViewUserControl(viewContext, viewUserControl);
                return;
            }

            throw new InvalidOperationException(
                String.Format(
                    CultureInfo.CurrentUICulture,
                    "The view at '{0}' must derive from ViewPage, ViewPage<TViewData>, ViewUserControl, or ViewUserControl<TViewData>.",
                    BaseView.ViewPath));
        }
    }
}