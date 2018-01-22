using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;

namespace Bzway.Database.Core
{
    public interface ISystemDatabase
    {
        IRepository<T> Entity<T>() where T : EntityBase, new();
    }
}
