using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using Bzway.Common.Share;
using System.Security.Principal;

namespace Bzway.Database.Core
{
    public interface IDatabaseProivder
    {
        ISystemDatabase GetDatabase(string connectionString, string databaseName);
    }
}