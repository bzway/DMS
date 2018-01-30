using Bzway.Framework.Application.Entity;
using System.Collections.Generic;
using System;

namespace Bzway.Framework.Application
{
    public interface ILoginService
    {
        Result<UserProfile> Login(string userName, string password, string validateCode);
    }
}
