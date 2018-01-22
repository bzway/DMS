#region License
// 

// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections;
using System.IO;
using System.Web.Mvc;
using Commons.Collections;
using NVelocity;
using NVelocity.App;
using NVelocity.Runtime;
using System.Web;
using NVelocity.Runtime.Resource.Loader;

namespace Bzway.Website.Sites.TemplateEngines.NVelocity.MvcViewEngine
{
    public class FileResourceLoaderEx : FileResourceLoader
    {
        public FileResourceLoaderEx() : base() { }
        private Stream FindTemplate(string filePath)
        {
            try
            {
                FileInfo file = new FileInfo(filePath);
                return new BufferedStream(file.OpenRead());
            }
            catch (Exception exception)
            {
                base.runtimeServices.Debug(string.Format("FileResourceLoader : {0}", exception.Message));
                return null;
            }
        }


        public override long GetLastModified(global::NVelocity.Runtime.Resource.Resource resource)
        {
            if (File.Exists(resource.Name))
            {
                FileInfo file = new FileInfo(resource.Name);
                return file.LastWriteTime.Ticks;
            }
            return base.GetLastModified(resource);
        }
        public override Stream GetResourceStream(string templateName)
        {
            if (File.Exists(templateName))
            {
                return FindTemplate(templateName);
            }
            return base.GetResourceStream(templateName);
        }
        public override bool IsSourceModified(global::NVelocity.Runtime.Resource.Resource resource)
        {
            if (File.Exists(resource.Name))
            {
                FileInfo file = new FileInfo(resource.Name);
                return (!file.Exists || (file.LastWriteTime.Ticks != resource.LastModified));
            }
            return base.IsSourceModified(resource);
        }
    }
}