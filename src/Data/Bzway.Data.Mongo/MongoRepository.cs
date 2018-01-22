using System;
using System.Collections.Generic;
using System.Linq;
using Bzway.Data.Core;
using Bzway.Data.Core.OpenExpressions;

namespace Bzway.Data.Mongo
{
    public class BaseEntityMongoRepository<T> : IRepository<T> where T : new()
    {
        #region ctor
        readonly string connectionString;
        readonly string datebaseName;
        readonly string collectionName;
        Schema schema { get; set; }
        public BaseEntityMongoRepository(string connectionString, string databaseName, Schema schema)
        {
            this.schema = schema;
            this.connectionString = connectionString;
            this.datebaseName = databaseName;
            this.collectionName = schema.Name;
        }

        #endregion

        #region Insert
        public virtual void Insert(T newData)
        {
            BsonDocument insert;
            if (newData is DynamicEntity)
            {
                var t = (DynamicEntity)Convert.ChangeType(newData, typeof(DynamicEntity));
                insert = GetInsertDocument(t);
            }
            else
            {
                insert = new BsonDocument();
                var uuid = newData.TryGetValue("Id");
                if (uuid == null || uuid.ToString() == string.Empty)
                {
                    insert.Add("Id", Guid.NewGuid().ToString("N"));
                }
                else
                {
                    insert.Add("Id", uuid.ToString());
                }
                insert.Add("CreatedOn", BsonValue.Create(newData.TryGetValue("CreatedOn")));
                insert.Add("UpdatedOn", BsonValue.Create(newData.TryGetValue("UpdatedOn")));
                insert.Add("CreatedBy", BsonValue.Create(newData.TryGetValue("CreatedBy")));
                insert.Add("UpdatedBy", BsonValue.Create(newData.TryGetValue("UpdatedBy")));
                insert.Add("Status", BsonValue.Create(newData.TryGetValue("Status")));
                foreach (var item in this.schema.AllColumns.Where(m => !m.IsSystemField))
                {
                    if (newData.TryGetValue(item.Name) != null)
                    {
                        insert.Add(item.Name, BsonValue.Create(newData.TryGetValue(item.Name)));
                    }
                    else
                    {
                        insert.Add(item.Name, BsonValue.Create(item.DefaultValue));
                    }
                }
            }

        
        }
        private BsonDocument GetInsertDocument(DynamicEntity newData)
        {
            var insert = new BsonDocument();
            if (string.IsNullOrEmpty(newData.Id ))
            {
                insert.Add("Id", Guid.NewGuid().ToString("N"));
            }
            else
            {
                insert.Add("Id", newData.Id );
            }
            insert.Add("CreatedOn", newData.CreatedOn);
            insert.Add("UpdatedOn", newData.UpdatedOn);
            insert.Add("CreatedBy", newData.CreatedBy);
            insert.Add("UpdatedBy", newData.UpdatedBy);
            insert.Add("Status", newData.Status);
            foreach (var item in this.schema.AllColumns.Where(m => !m.IsSystemField))
            {
                if (newData.ContainsKey(item.Name))
                {
                    insert.Add(item.Name, BsonValue.Create(newData[item.Name]));
                }
                else
                {
                    insert.Add(item.Name, BsonValue.Create(item.DefaultValue));
                }
            }
            return insert;
        }
        #endregion

        #region Delete
        public virtual void Delete(T oldData)
        {
            string uuid;
            if (oldData is DynamicEntity)
            {
                var t = (DynamicEntity)Convert.ChangeType(oldData, typeof(DynamicEntity));
                uuid = t.Id ;
            }
            else
            {
                var t = oldData.TryGetValue("uuid");
                if (t == null)
                {
                    return;
                }
                uuid = t.ToString();
            }
            this.Delete(uuid);
        }
        public void Delete(string uuid)
        {
          
        }
        #endregion

        #region Update
        public virtual void Update(T newData, string uuid = "")
        {
           
        }
        public IUpdate<T> Update(IWhereExpression where)
        {
            return new OpenUpdate<T>(this, where);
        }
        public bool Execute(IUpdate<T> update)
        {
            return true;
        }

        public IOpenQuery<T> Query(params string[] fields)
        {
            throw new NotImplementedException();
        }

        public IWhere<T> Filter()
        {
            throw new NotImplementedException();
        }

        public object Execute(IOpenQuery<T> query)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Query

        #endregion

    }
    
    public class EntityMongoRepository<T> : BaseEntityMongoRepository<T> where T : EntityBase, new()
    {
        public EntityMongoRepository(string connectionString, string databaseName, Schema schema)
            : base(connectionString, databaseName, schema)
        {

        }
    }
    public class DynamicEntityMongoRepository<T> : BaseEntityMongoRepository<T> where T : DynamicEntity, new()
    {
        public DynamicEntityMongoRepository(string connectionString, string databaseName, Schema schema)
            : base(connectionString, databaseName, schema)
        {
        }
    }
}