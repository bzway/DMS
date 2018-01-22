using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using Bzway.Database.Core;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Newtonsoft.Json;
using Bzway.Common.Share;
using System.Reflection;
using System.Threading;
using System.Security.Principal;

namespace Bzway.Database.File
{
    internal static class OpenEntityHelper
    {
        public static object TryGetValue(this object entity, string name)
        {
            try
            {
                PropertyInfo info = entity.GetType().GetProperties().Where(m => m.Name == name && m.CanRead).First();
                return info.GetValue(entity, null);
            }
            catch
            {
                return null;
            }
        }
        public static void TrySetValue(this object entity, string name, object value)
        {
            try
            {
                PropertyInfo info = entity.GetType().GetProperties().Where(m => m.Name == name && m.CanRead).First();
                info.SetValue(entity, value);
            }
            catch
            {
            }
        }
    }
    public class FileDatabase : SystemDatabase
    {
        readonly IFileProvider fileProvider;
        public FileDatabase(string root = "")
        {
            if (string.IsNullOrEmpty(root))
            {
                root = Path.Combine(Directory.GetCurrentDirectory(), "../../../", "data", "server", "config");
            }
            else
            {
                root = Path.Combine(Directory.GetCurrentDirectory(), "../../../", "data", "sites", root);
            }
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            this.fileProvider = new PhysicalFileProvider(root);
        }

        public override IRepository<T> Entity<T>()
        {
            return new FileRepository<T>(this.fileProvider, AppEngine.GetService<IPrincipal>());
        }
    }
}