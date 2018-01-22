using Autofac;
using Bzway.Common.Share;
using Bzway.Database.Core;

namespace Bzway.Database.File
{
    public class FileDatabaseRegister : IDependencyRegister
    {
        public const string ProviderName = "File";
        public int Order => 1;

        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<FileDatabaseProvider>().As<IDatabaseProivder>().Named<IDatabaseProivder>(ProviderName);
        }
    }
}
