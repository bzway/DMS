using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using System.Linq;
using Microsoft.Extensions.DependencyModel;

namespace Bzway.Common.Share
{
  
    public interface ITypeFinder
    {
        IEnumerable<T> Find<T>(params string[] paths);
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
        List<Type> types = new List<Type>();

        IEnumerable<Type> Find(Type fromType, params string[] paths)
        {
            foreach (var item in paths)
            {
                try
                {
                    types.AddRange(Assembly.Load(AssemblyLoadContext.GetAssemblyName(item)).GetTypes());
                }
                catch { }
            }
            return this.types.Where(t => fromType.IsAssignableFrom(t) && fromType != t);
        }
        public IEnumerable<T> Find<T>(params string[] paths)
        {
            List<T> list = new List<T>();
            foreach (var item in this.Find(typeof(T), paths))
            {
                try
                {
                    list.Add((T)Activator.CreateInstance(item));
                }
                catch { }
            }
            return list;
        }
    }
}
