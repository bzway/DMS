using Bzway.Common.Share;
using Bzway.Common.Utility;
using Bzway.Database.Core;
using Bzway.Framework.Application.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Autofac;

namespace Bzway.Framework.Application
{
    public interface ILoginProvider
    {
        ILoginService TryResolveService(string Name);
    }

    public class LoginProvider : IDependencyRegister, ILoginProvider
    {
        #region ctor
        /// <summary>
        /// 终端凭证，所有用户的调用凭证
        /// </summary>
        const string ClientCredential = "client_credential";
        /// <summary>
        /// 应用凭证，所有应用的调用凭证
        /// </summary>
        const string AppCredential = "app_credential";
        /// <summary>
        /// 用户授权凭证，只有合法的应用，在用户的许可上才能使用
        /// </summary>
        const string AuthCredential = "auth_credential";

        public int Order => 1;
        #endregion
        public ILoginService TryResolveService(string Name)
        {
            return AppEngine.GetService<ILoginService>(Name);
        }
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<UserLoginService>().As<ILoginService>().Named<ILoginService>(LoginProvider.ClientCredential);
            builder.RegisterType<AppLoginService>().As<ILoginService>().Named<ILoginService>(LoginProvider.AppCredential);
            builder.RegisterType<AuthLoginService>().As<ILoginService>().Named<ILoginService>(LoginProvider.AuthCredential);
            builder.RegisterType<LoginProvider>().As<ILoginProvider>();
        }
    }
}