using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using Bzway.Common.Share;
using System.Security.Principal;

namespace Bzway.Database.Core
{
    public abstract class SystemDatabase : ISystemDatabase
    {
        public static ISystemDatabase GetDatabase(string providerName = "", string connectionString = "", string databaseName = "")
        {
            var databaseProvider = AppEngine.GetService<IDatabaseProivder>(providerName);
            if (databaseProvider == null)
            {
                throw new Exception($"proivder:{providerName} is not supported");
            }
            return databaseProvider.GetDatabase(connectionString, databaseName);
        }
        public abstract IRepository<T> Entity<T>() where T : EntityBase, new();
    }
}