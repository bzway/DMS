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


    public class DefaultRegister  : IDependencyRegister
    {
        #region ctor
        public int Order => 1;
        #endregion

        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<SiteService>().As<ISiteService>();
        }
    }
}