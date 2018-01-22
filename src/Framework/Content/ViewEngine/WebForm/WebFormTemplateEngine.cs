#region License
// 

// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bzway.Website.Sites.View.CodeSnippet;
using Bzway.Website.Sites.Models;
using System.Web.Mvc;
using Bzway.Website.Sites.View;


namespace Bzway.Website.Web.TemplateEngines.WebForm
{
    public class WebFormTemplateEngine : ITemplateEngine
    {
        public IViewEngine GetViewEngine()
        {
            return new WebFormViewEngine();
        }
        public IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new WebFormView(controllerContext, viewPath, masterPath);
        }

        public IEnumerable<string> FileExtensions
        {
            get { return new string[] { ".ascx", ".aspx" }; }
        }

        public string Name
        {
            get
            {
                return "WebForm";
            }
        }

        public string GetFileExtensionForLayout()
        {
            return ".aspx";
        }

        public string GetFileExtensionForView()
        {
            return ".ascx";
        }

        public IDataRuleCodeSnippet GetDataRuleCodeSnippet(TakeOperation takeOperation)
        {
            switch (takeOperation)
            {
                case TakeOperation.List:
                    return new WebFormListCodeSnippet();

                case TakeOperation.First:
                    return new WebFormDetailCodeSnippet();
                default:
                    return null;
            }
        }

        public ILayoutPositionParser GetLayoutPositionParser()
        {
            return new WebFormLayoutPositionParser();
        }


        public ICodeHelper GetCodeHelper()
        {
            return new WebFormCodeHelper();
        }


        public int Order
        {
            get { return 2; }
        }
    }

}
