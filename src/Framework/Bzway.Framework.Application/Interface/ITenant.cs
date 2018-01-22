using Bzway.Database.Core;
using Bzway.Framework.Application.Entity;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Principal;

namespace Bzway.Framework.Application
{
    public interface ITenant
    {
        HttpContext Context { get; }
        Site Site { get; }
    }
}
