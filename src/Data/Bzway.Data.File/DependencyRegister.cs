using Autofac;
using Bzway.Data.Core;
using Bzway.Common.Share;

namespace Bzway.Data.JsonFile
{
    public class DependencyRegister : IDependencyRegister
    {
        public int Order => 1;

        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<FileDatabase>().As<IDatabase>().Named<IDatabase>("Default");
        }
    }
}
