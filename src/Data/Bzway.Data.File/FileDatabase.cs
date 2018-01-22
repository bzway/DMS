using Bzway.Data.Core;

namespace Bzway.Data.JsonFile
{
    public class FileDatabase : OpenDatabase, IDatabase
    {
        public FileDatabase(string connectionString = "", string databaseName = "")
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
            return new BaseEntityJsonFileRepository<T>(this.ConnectionString, this.DatabaseName, schema);
        }
        public override IRepository<DynamicEntity> DynamicEntity(Schema schema)
        {
            return new DynamicEntityJsonFileRepository<DynamicEntity>(this.ConnectionString, this.DatabaseName, schema);
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
            return new EntityJsonFileRepository<T>(this.ConnectionString, this.DatabaseName, schema);
        }
        public override IDatabase Clone(string ConnectionString, string DatabaseName)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                return new FileDatabase("Master","Default");
            }
            if (string.IsNullOrEmpty(DatabaseName))
            {
                return new FileDatabase(ConnectionString,"Default");
            }
            return new FileDatabase(ConnectionString, DatabaseName);
        }
    }
}