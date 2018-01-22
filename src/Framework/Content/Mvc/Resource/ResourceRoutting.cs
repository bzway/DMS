using OpenData.Web.Mvc.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using System.Web.Mvc;
using System.Web;
using System.IO;
using OpenData.Utility;
using System.IO.Compression;
using OpenData.Utility;

namespace OpenData.Web.Mvc.Resource
{
    public class ResourceRoutting : Controller, IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
               "WebResource",
               "WebResource/{name}/{version}/{condition}",
               new { controller = "ExternalResource", action = "Index", condition = "" }
           );
        }

        public int Priority
        {
            get
            {
                return 10;
            }
        }

        public static List<string> list = new List<string>();


        public ActionResult Index(string name, string version, string condition)
        {
            if (list.Contains(name))
            {
                return File("", "");
            }
            return File("", "");

        }
    }
    public static class SSS
    {
        public static IHtmlString ExternalResources(this HtmlHelper htmlHelper, RouteValueDictionary htmlAttributes, params string[] files)
        {

            string key = Cryptor.EncryptMD5(string.Join(";", files));
            if (ResourceRoutting.list.Contains(key))
            {
                return new HtmlString("");
            }

            string output = key;

            // Combine
            using (StreamWriter sw = new StreamWriter(output))
            {
                foreach (var fileInfo in files)
                {
                    string content = System.IO.File.ReadAllText(htmlHelper.ViewContext.HttpContext.Server.MapPath(fileInfo));


                    content = CSSMinify.Minify(null, fileInfo, "", content);
                    sw.WriteLine(content.Trim());
                }
            }
            // Combine
            using (StreamWriter sw = new StreamWriter(output))
            {
                foreach (var fileInfo in files)
                {
                    string content = System.IO.File.ReadAllText(htmlHelper.ViewContext.HttpContext.Server.MapPath(fileInfo));


                    content = CSSMinify.Minify(null, fileInfo, "", content);
                    sw.WriteLine(content.Trim());
                }
            }
            return new HtmlString("");
        }
    }
}