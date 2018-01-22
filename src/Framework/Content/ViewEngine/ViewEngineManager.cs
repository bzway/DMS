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
using System.IO;
using System.Web.Mvc;
using OpenData.Framework.Common.ViewEngine.Razor;

namespace OpenData.Framework.Common.ViewEngine
{
    public interface IBzwayViewEngine
    {

        IViewEngine GetViewEngine();

        IView GetView(ControllerContext controllerContext, string viewVirtualPath, string masterVirtualPath);

        IEnumerable<string> FileExtensions { get; }
    }
    public static class ViewEngineManager
    {
        static List<IBzwayViewEngine> engines = new List<IBzwayViewEngine>();
        static ViewEngineManager()
        {
            //RegisterEngine(new WebFormViewEngine());
            RegisterEngine(new BzwayRazorEngine());
        }
        static void RegisterEngine(IBzwayViewEngine engine)
        {
            lock (engines)
            {
                engines.Add(engine);
            }
        }
        public static IBzwayViewEngine GetEngineByFileExtension(string fileExtension)
        {
            var engine = engines.Where(it => it.FileExtensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase)).FirstOrDefault();
            if (engine == null)
            {
                throw new NotSupportedException(string.Format("Not supported engine for '{0}'", fileExtension));
            }
            return engine;
        }
        public static IBzwayViewEngine GetEngineByName(string name)
        {
            var engine = engines.Where(it => it.ToString().IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0).FirstOrDefault();
            if (engine == null)
            {
                throw new NotSupportedException(string.Format("Not found the engine. Name:'{0}'", name));
            }
            return engine;
        }
    }
}