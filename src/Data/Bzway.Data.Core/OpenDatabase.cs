using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Bzway.Common.Share;
using Autofac;
namespace Bzway.Data.Core
{
    public abstract class OpenDatabase : IDatabase
    {
        #region ctor
        static Dictionary<string, Dictionary<string, Schema>> cache = new Dictionary<string, Dictionary<string, Schema>>();
        protected string ConnectionString;
        protected string DatabaseName;
        protected Dictionary<string, Schema> dictionary
        {
            get
            {
                //通过数据库链接从本地缓存中获得Schema
                var key = this.ConnectionString + this.DatabaseName;
                if (!cache.ContainsKey(key))
                {
                    //从数据库中获得Schema
                    var dict = new Dictionary<string, Schema>();
                    var list = this.DynamicEntity(Schema.SchemaSchema).Query().ToList();
                    foreach (var item in list)
                    {
                        Schema schema = Schema.EntitySchema(item["Name"].ToString(), item["Description"] == null ? null : item["Description"].ToString());

                        var colList = this.DynamicEntity(Schema.ColumnSchema).Query().Where("SchemaName", schema.Name, CompareType.Equal).ToList();
                        foreach (dynamic column in colList)
                        {
                            schema.AddColumn(new Column()
                            {
                                //AllowNull = (bool)column.AllowNull,
                                //ControlType = (string)column.ControlType,
                                //DefaultValue = (string)column.DefaultValue,
                                //Indexable = (bool)column.Indexable,
                                //IsSystemField = (bool)column.IsSystemField,
                                //Label = (string)column.Label,
                                //Length = (int)column.Length,
                                //Modifiable = (bool)column.Modifiable,
                                //Name = (string)column.Name,
                                //Order = (int)column.Order,
                                //ShowInGrid = (bool)column.ShowInGrid,
                                //Tooltip = (string)column.Tooltip,
                            });
                        }
                        dict.Add(schema.Name, schema);
                    }
                    //将Schema缓存到本地
                    cache.Add(key, dict);
                }
                return cache[key];
            }
            set
            {
                var key = this.ConnectionString + this.DatabaseName;
                cache[key] = value;
            }
        }
        public static IDatabase GetDatabase(string providerName = "", string ConnectionString = "", string DatabaseName = "")
        {
            if (string.IsNullOrEmpty(providerName))
            {
                providerName = "Default";
            }
            var db = AppEngine.Current.ResolveNamed<IDatabase>(providerName);
            return db.Clone(ConnectionString, DatabaseName);
        }
        public abstract IDatabase Clone(string ConnectionString, string DatabaseName);
        #endregion

        #region Entity
        public abstract IRepository<DynamicEntity> DynamicEntity(Schema schema);

        public abstract IRepository<T> BaseEntity<T>() where T : new();

        public abstract IRepository<T> Entity<T>() where T : EntityBase, new();
        #endregion

        #region Schema
        protected Schema GetSchemaFromEntity(Type type, string entityName)
        {
            var schema = Schema.EntitySchema(entityName, "");
            var props = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
            foreach (var item in props)
            {
                var attribute = item.GetCustomAttribute<ColumnAttribute>();
                if (attribute == null)
                {
                    attribute = new ColumnAttribute(item.Name)
                    {
                        //AllowNull = !item.GetType(),
                        DefaultValue = string.Empty,
                        Indexable = false,
                        Label = item.Name,
                        Modifiable = true,
                        Name = item.Name,
                        ShowInGrid = true,
                        Order = 0,
                        Length = 0,
                        RegExp = string.Empty,
                        Tooltip = string.Empty,
                        ControlType = "TextBox",
                    };
                }
                schema.AddColumn(new Column()
                {
                    AllowNull = attribute.AllowNull,
                    ControlType = attribute.ControlType,
                    DefaultValue = attribute.DefaultValue,
                    Indexable = attribute.Indexable,
                    Label = string.IsNullOrEmpty(attribute.Label) ? item.Name : attribute.Label,
                    Length = attribute.Length,
                    Modifiable = attribute.Modifiable,
                    Name = string.IsNullOrEmpty(attribute.Name) ? item.Name : attribute.Name,
                    Order = attribute.Order,
                    ShowInGrid = attribute.ShowInGrid,
                    Tooltip = attribute.Tooltip,
                    IsSystemField = true,
                });
            }
            this.RefreshSchema(schema);
            this.dictionary[schema.Name] = schema;
            return schema;
        }
        public virtual bool RefreshSchema(Schema item)
        {
            RemoveSchema(item);
            try
            {
                var newEntity = new DynamicEntity();
                newEntity["Name"] = item.Name;
                newEntity["Description"] = item.Description;
                this.DynamicEntity(Schema.SchemaSchema).Insert(newEntity);
                foreach (var column in item.AllColumns)
                {
                    var newColumnEntity = new DynamicEntity();
                    newColumnEntity["SchemaName"] = item.Name;
                    newColumnEntity["AllowNull"] = column.AllowNull;
                    newColumnEntity["ControlType"] = column.ControlType;
                    newColumnEntity["DefaultValue"] = column.DefaultValue;
                    newColumnEntity["Indexable"] = column.Indexable;
                    newColumnEntity["IsSystemField"] = column.IsSystemField;
                    newColumnEntity["Label"] = column.Label;
                    newColumnEntity["Length"] = column.Length;
                    newColumnEntity["Modifiable"] = column.Modifiable;
                    newColumnEntity["Name"] = column.Name;
                    newColumnEntity["Order"] = column.Order;
                    newColumnEntity["ShowInGrid"] = column.ShowInGrid;
                    newColumnEntity["Tooltip"] = column.Tooltip;
                    this.DynamicEntity(Schema.ColumnSchema).Insert(newColumnEntity);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool RemoveSchema(Schema item)
        {
            try
            {
                var entitySchema = this.DynamicEntity(Schema.SchemaSchema).Query().Where("Name", item.Name, CompareType.Equal).First();
                if (entitySchema == null)
                {
                    return false;
                }

                foreach (var column in this.DynamicEntity(Schema.ColumnSchema).Query().Where("SchemaName", item.Name, CompareType.Equal).ToList())
                {
                    this.DynamicEntity(Schema.ColumnSchema).Delete(column);
                }

                this.DynamicEntity(Schema.SchemaSchema).Delete(entitySchema);

                if (this.dictionary.ContainsKey(item.Name))
                {
                    this.dictionary.Remove(item.Name);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public Schema this[string key]
        {
            get
            {
                if (this.dictionary.ContainsKey(key))
                {
                    return this.dictionary[key];
                }
                else
                {
                    return null;
                }
            }
        }

        public IEnumerator<Schema> GetEnumerator()
        {
            return this.dictionary.Values.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.dictionary.Values.GetEnumerator();
        }
        #endregion
        public void Dispose()
        {
        }


        public ITransaction GetTransaction()
        {
            return null;
        }

        public void BeginTransaction()
        {
        }

        public void AbortTransaction()
        {

        }
    }
}