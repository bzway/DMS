using System;
using Bzway.Database.Core;
using System.Security.Principal;

namespace Bzway.Database.Mongo
{

    public class MongoDatabase : SystemDatabase
    {
        readonly string url;
        readonly string database;
        public MongoDatabase(string url, string database)
        {
            this.url = url;
            this.database = database;
        }
        public override IRepository<T> Entity<T>()
        {
            return new MongoRepository<T>(this.url, this.database, AppEngine.GetService<IPrincipal>());
        }
    }
}