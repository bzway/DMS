using Bzway.Data.Core;
using System.Reflection; 
namespace Bzway.Data.Mongo
{
    public class MongoDatabase : OpenDatabase, IDatabase
    {
        public MongoDatabase(string connectionString = "mongodb://localhost:27017", string databaseName = "master")
        {
            this.ConnectionString = connectionString;
            this.DatabaseName = databaseName;
        }
        public override IRepository<T> BaseEntity<T>()
        {
            var type = typeof(T);
            var entityName = type.Name;
            var schema = this[entityName];
            if (schema == null)
            {
                schema = GetSchemaFromEntity(type, entityName);
            }
            return new BaseEntityMongoRepository<T>(this.ConnectionString, this.DatabaseName, schema);
        }
        public override IRepository<DynamicEntity> DynamicEntity(Schema schema)
        {
            return new DynamicEntityMongoRepository<DynamicEntity>(this.ConnectionString, this.DatabaseName, schema);
        }
        public override IRepository<T> Entity<T>()
        {
            var type = typeof(T);
            var entityName = type.Name;
            var schema = this[entityName];
            if (schema == null)
            {
                schema = GetSchemaFromEntity(type, entityName);
            }
            return new EntityMongoRepository<T>(this.ConnectionString, this.DatabaseName, schema);
        }
        public override IDatabase Clone(string ConnectionString, string DatabaseName)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                return new MongoDatabase();
            }
            if (string.IsNullOrEmpty(DatabaseName))
            {
                return new MongoDatabase(ConnectionString);
            }
            return new MongoDatabase(ConnectionString, DatabaseName);
        }
    }
}