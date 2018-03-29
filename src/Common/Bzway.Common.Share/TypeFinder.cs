using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using System.Linq;
using Microsoft.Extensions.DependencyModel;
using System.IO;

namespace Bzway.Common.Share
{

    public interface ITypeFinder
    {
        IEnumerable<Type> Find<T>(params string[] paths);
        IEnumerable<Type> Find(Type fromType, params string[] paths);
    }
    public class AssemblyLoader : AssemblyLoadContext
    {
        // Not exactly sure about this
        protected override Assembly Load(AssemblyName assemblyName)
        {
            var assembly = Assembly.Load(new AssemblyName(DependencyContext.Default.CompileLibraries.Where(d => d.Name.Contains(assemblyName.Name)).First().Name));
            return assembly;
        }
    }
    public class TypeFinder : ITypeFinder
    {
        static Dictionary<string, List<Type>> typeCache = new Dictionary<string, List<Type>>();

        public IEnumerable<Type> Find(Type fromType, params string[] paths)
        {
            if (paths.Length == 0)
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "bin");
                paths = Directory.GetFiles(folder, "*.dll", SearchOption.AllDirectories);
            }
            List<Type> types = new List<Type>();

            foreach (var item in paths)
            {
                if (!typeCache.ContainsKey(item))
                {
                    List<Type> list = new List<Type>();
                    list.AddRange(Assembly.Load(AssemblyLoadContext.GetAssemblyName(item)).GetTypes());
                    typeCache.Add(item, list);
                }
                types.AddRange(typeCache[item]);
            }
            return types.Where(t => fromType.IsAssignableFrom(t) && fromType != t);
        }
        public IEnumerable<Type> Find<T>(params string[] paths)
        {
            return this.Find(typeof(T), paths);
        }
    }
}
