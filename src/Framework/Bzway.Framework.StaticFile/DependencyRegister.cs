using Autofac;
using Bzway.Common.Share;

namespace Bzway.Framework.StaticFile
{
    public class DependencyRegister : IDependencyRegister
    {
        public int Order => 1;

        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<StaticFileService>().As<IStaticFileService>();
        }
    }
}