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
        ILoginService Get(string Name);
    }

    public class LoginProvider : ILoginProvider 
    {
        public const string EmailLoginService = "EmailLoginService";
        public const string MobileLoginService = "MobileLoginService";
        public const string CardLoginService = "CardLoginService";
        public ILoginService Get(string Name)
        {
            return AppEngine.GetService<ILoginService>(Name);
        }
    }
}