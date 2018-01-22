using Autofac;
using Bzway.Common.Share;

namespace Bzway.Module.Wechat.Wechat
{
    public class DependencyRegister : IDependencyRegister
    {
        public int Order => 1;

        public void Register(ContainerBuilder builder)
        {
           
        }
    }
}