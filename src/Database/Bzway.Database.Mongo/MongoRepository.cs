using System;
using System.Linq;
using Bzway.Database.Core;
using Bzway.Common.Share;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Bzway.Database.Mongo
{
    internal class MongoRepository<T> : IRepository<T> where T : EntityBase, new()
    {
        readonly IMongoClient client;
        readonly Type type;
        readonly IPrincipal user;
        readonly IMongoDatabase database;


        public MongoRepository(string url, string database, IPrincipal user)
        {
            this.type = typeof(T);
            this.user = user;
            if (string.IsNullOrEmpty(url))
            {
                this.client = new MongoClient();
            }
            else
            {
                this.client = new MongoClient(url);
            }
            this.database = client.GetDatabase(database);
            var cache = CacheManager.Default.MemCacheProvider;
        }
        public IQueryable<T> Query()
        {
            var collection = this.database.GetCollection<T>(this.type.Name);
            return collection.AsQueryable();
        }


        public void Delete(T newData)
        {
            this.Delete(newData.Id);
        }

        public void Delete(string uuid)
        {
            var collection = this.database.GetCollection<T>(this.type.Name);
            collection.DeleteOne(m => m.Id == uuid);
        }

        public bool Execute(ICommand<T> command)
        {
            return true;
        }

        public IWhere<T> Filter()
        {
            throw new NotImplementedException();
        }

        public void Insert(T newData)
        {
            if (string.IsNullOrEmpty(newData.Id))
            {
                newData.Id = Guid.NewGuid().ToString("N");
            }
            newData.CreatedBy = newData.UpdatedBy = this.user.Identity.Name;
            newData.CreatedOn = newData.UpdatedOn = DateTime.UtcNow;
            var collection = this.database.GetCollection<T>(this.type.Name);
            collection.InsertOne(newData);
        }
        public void Update(IUpdate<T> update, IWhere<T> where)
        {
            throw new NotImplementedException();
        }
        public void Update(T newData, string uuid = "")
        {
            if (string.IsNullOrEmpty(uuid))
            {
                uuid = newData.Id;
            }
            else
            {
                newData.Id = uuid;
                newData.UpdatedBy = this.user.Identity.Name;
                newData.UpdatedOn = DateTime.UtcNow;
            }
            var collection = this.database.GetCollection<T>(this.type.Name);
            collection.UpdateOne(m => m.Id == uuid, newData.ToBsonDocument());
        }
    }
}