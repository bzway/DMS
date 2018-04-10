using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Bzway.Data.Core;
using Bzway.Data.Core.OpenExpressions;
using System.Linq.Expressions;
using Bzway.Common.Utility;
using Bzway.Common.Collections;
using Microsoft.Extensions.Configuration;

namespace Bzway.Data.JsonFile
{
    public class BaseEntityJsonFileRepository<T> : IRepository<T> where T : new()
    {
        #region Ctor
        readonly string root;
        readonly string entityName;
        readonly Schema schema;
        public BaseEntityJsonFileRepository(string connectionString, string databaseName, Schema schema)
        {
            this.entityName = schema.Name;
            this.schema = schema;
            this.root = Path.Combine(Directory.GetCurrentDirectory(), AppEngine.Default.DataFolder, connectionString, databaseName, entityName);
            if (!Directory.Exists(this.root))
            {
                Directory.CreateDirectory(this.root);
            }
        }
        #endregion

        #region protected
        protected List<T> GetData()
        {
            var filePath = Path.Combine(this.root, "data.json");
            List<T> list;
            if (!File.Exists(filePath))
            {
                using (File.Create(filePath))
                {

                };
            }
            list = SerializationHelper.DeserializeObjectJson<List<T>>(File.ReadAllText(filePath));
            if (list == null)
            {
                list = new List<T>();
            }
            return list;
        }
        protected void SetData(List<T> list)
        {
            var filePath = Path.Combine(this.root, "data.json");
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }
            File.WriteAllText(filePath, SerializationHelper.SerializeObjectToJson(list));
        }
        private Func<T, bool> GetFilter(IWhereExpression where)
        {
            Func<T, bool> filter = null;


            if (where is WhereExpression)
            {
                filter = GetAndFilter((WhereExpression)where);
            }
            else
            {
                filter = GetOrFilter((OrAlsoExpression)where);
            }

            if (filter == null)
            {
                filter = new Func<T, bool>(m => { return true; });
            }
            return filter;
        }
        private Func<T, bool> GetAndFilter(WhereExpression where)
        {
            Func<T, bool> filter = null;
            switch (where.CompareType)
            {
                case CompareType.Equal:
                    filter = new Func<T, bool>((target) =>
                    {
                        var result = object.Equals(target.TryGetValue(where.FieldName), where.Value);
                        return result;
                    });
                    break;
                case CompareType.NotEqual:
                    filter = new Func<T, bool>((target) =>
                    {
                        var result = !object.Equals(target.TryGetValue(where.FieldName), where.Value);
                        return result;
                    });
                    break;
                case CompareType.Like:
                    if (where.Value != null)
                    {
                        filter = new Func<T, bool>((target) => { return target.TryGetValue(where.FieldName).ToString().Contains(where.Value.ToString()); });
                    }
                    else
                    {
                        filter = new Func<T, bool>((target) => { return true; });
                    }
                    break;
                case CompareType.GreaterThan:
                    filter = new Func<T, bool>((target) =>
                    {
                        var result = object.Equals(target.TryGetValue(where.FieldName), where.Value);
                        return false;
                    });
                    break;

                case CompareType.GreaterThanOrEqual:
                    filter = new Func<T, bool>((target) =>
                    {
                        var result = object.Equals(target.TryGetValue(where.FieldName), where.Value);
                        return false;
                    });
                    break;
                case CompareType.LessThan:
                    filter = new Func<T, bool>((target) =>
                    {
                        var result = object.Equals(target.TryGetValue(where.FieldName), where.Value);
                        return false;
                    });
                    break;

                case CompareType.LessThanOrEqual:
                    filter = new Func<T, bool>((target) =>
                    {
                        var result = object.Equals(target.TryGetValue(where.FieldName), where.Value);
                        return false;
                    });
                    break;
                case CompareType.Startwith:
                    if (where.Value != null)
                    {
                        filter = new Func<T, bool>((target) => { return (target.TryGetValue(where.FieldName).ToString().StartsWith(where.Value.ToString())); });

                    }
                    else
                    {
                        filter = new Func<T, bool>((target) => { return true; });
                    }

                    break;
                case CompareType.EndWith:
                    if (where.Value != null)
                    {
                        filter = new Func<T, bool>((target) => { return (target.TryGetValue(where.FieldName).ToString().EndsWith(where.Value.ToString())); });

                    }
                    else
                    {
                        filter = new Func<T, bool>((target) => { return true; });
                    }

                    break;
                case CompareType.Contains:
                    if (where.Value != null)
                    {
                        filter = new Func<T, bool>((target) =>
                        {
                            if (target.TryGetValue(where.FieldName) == null)
                            {
                                return true;
                            }
                            return where.Value.ToString().Contains(target.TryGetValue(where.FieldName).ToString());
                        });

                    }
                    else
                    {
                        filter = new Func<T, bool>((target) => { return true; });
                    }

                    break;
                case CompareType.NoLike:
                    if (where.Value != null)
                    {
                        filter = new Func<T, bool>((target) => { return !(target.TryGetValue(where.FieldName).ToString().Contains(where.Value.ToString())); });
                    }
                    else
                    {
                        filter = new Func<T, bool>((target) => { return true; });
                    }
                    break;
                default:
                    filter = new Func<T, bool>((target) => { return true; });
                    break;
            }
            if (where.Expression == null)
            {
                return filter;
            }
            if (where.Expression is OrAlsoExpression)
            {
                return new Func<T, bool>((target) => { return filter(target) | GetOrFilter((OrAlsoExpression)where.Expression)(target); });
            }
            else
            {
                return new Func<T, bool>((target) => { return filter(target) & GetAndFilter((WhereExpression)where.Expression)(target); });
            }
        }
        private Func<T, bool> GetOrFilter(OrAlsoExpression or)
        {
            Func<T, bool> filterLeft;
            if (or.Left is WhereExpression)
            {
                filterLeft = GetAndFilter((WhereExpression)or.Left);
            }
            else
            {
                filterLeft = GetOrFilter((OrAlsoExpression)or.Left);
            }
            Func<T, bool> filterRight;
            if (or.Right is WhereExpression)
            {
                filterRight = GetAndFilter((WhereExpression)or.Right);
            }
            else
            {
                filterRight = GetOrFilter((OrAlsoExpression)or.Right);
            }
            return new Func<T, bool>((target) => { return filterLeft(target) | filterRight(target); });

        }
        #endregion

        #region Insert
        public virtual void Insert(T newData)
        {
            if (newData is DynamicEntity)
            {
                var t = (DynamicEntity)Convert.ChangeType(newData, typeof(DynamicEntity));
                Insert(t);
                return;
            }
            if (newData is EntityBase)
            {
                var t = (EntityBase)(object)newData;
                Insert(t);
                return;
            }
            var uuid = newData.TryGetValue("Id");
            if (uuid == null || uuid.ToString() == string.Empty)
            {
                newData.TrySetValue("Id", Guid.NewGuid().ToString("N"));
            }
            newData.TrySetValue("CreatedOn", DateTime.UtcNow);
            newData.TrySetValue("UpdatedOn", DateTime.UtcNow);
            var list = this.GetData();
            list.Add(newData);
            this.SetData(list);
        }
        private void Insert(DynamicEntity newData)
        {
            if (string.IsNullOrEmpty(newData.Id))
            {
                newData.Id = Guid.NewGuid().ToString("N");
            }
            newData.CreatedOn = DateTime.UtcNow;
            newData.UpdatedOn = DateTime.UtcNow;
            List<T> list = this.GetData();
            var t = (T)Convert.ChangeType(newData, typeof(T));
            list.Add(t);
            this.SetData(list);
        }
        private void Insert(EntityBase newData)
        {
            if (string.IsNullOrEmpty(newData.Id))
            {
                newData.Id = Guid.NewGuid().ToString("N");
            }
            newData.CreatedOn = DateTime.UtcNow;
            newData.UpdatedOn = DateTime.UtcNow;
            var list = this.GetData();
            var t = (T)Convert.ChangeType(newData, typeof(T));
            list.Add(t);
            this.SetData(list);
        }
        #endregion

        #region Delete
        public virtual void Delete(T oldData)
        {
            var uuid = oldData.TryGetValue("Id");
            if (uuid == null || uuid.ToString() == string.Empty)
            {
                return;
            }
            this.Delete(uuid.ToString());
        }
        public virtual void Delete(string uuid)
        {
            if (string.IsNullOrEmpty(uuid))
            {
                return;
            }
            var list = this.GetData();
            T existedData = default(T);
            foreach (var item in list)
            {
                if (item.TryGetValue("Id").ToString() == uuid)
                {
                    existedData = item;
                    break;
                }
            }

            list.Remove(existedData);
            this.SetData(list);
        }

        #endregion

        #region Update
        public virtual void Update(T newData, string uuid = "")
        {
            if (string.IsNullOrEmpty(uuid))
            {
                uuid = newData.TryGetValue("Id").ToString();
            }

            if (string.IsNullOrEmpty(uuid))
            {
                return;
            }

            newData.TrySetValue("UpdatedOn", DateTime.UtcNow);


            List<T> list = this.GetData();
            if (list == null || list.Count < 1)
            {
                return;
            }
            var oldData = list.FirstOrDefault(m => m.TryGetValue("Id").ToString() == uuid);
            if (oldData == null)
            {
                return;
            }
            list.Remove(oldData);
            list.Add((T)newData);
            this.SetData(list);
        }
        public IUpdate<T> Update(IWhereExpression where)
        {
            return new OpenUpdate<T>(this, where);
        }
        public bool Execute(IUpdate<T> update)
        {
            if (update.WhereExpression == null)
            {
                return false;
            }




            Func<T, bool> predicate = this.GetFilter(update.WhereExpression);

            var q = this.GetData().AsEnumerable().Where(predicate);

            var list = q.ToList();

            foreach (var newData in list)
            {
                newData.TrySetValue("UpdatedOn", DateTime.UtcNow);
                var updateExpression = update.UpdateExpression;
                while (updateExpression != null)
                {
                    newData.TrySetValue(updateExpression.FieldName, updateExpression.Value);
                    updateExpression = update.UpdateExpression;
                }
            }
            this.SetData(list);
            return true;
        }
        #endregion

        #region Query
        public IWhere<T> Filter()
        {
            return new OpenWhere<T>();
        }
        public IOpenQuery<T> Query(params string[] fields)
        {
            return new OpenQuery<T>(this, fields);
        }
        public object Execute(IOpenQuery<T> query)
        {
            switch (query.CallExpression.CallType)
            {
                case CallType.Count:
                    return Count(query);
                case CallType.First:
                    return First(query);
                case CallType.Last:
                    return Last(query);
                case CallType.ToList:
                    return ToList(query);
                case CallType.PageList:
                    return PageList(query);
                default:
                    return null;
            }
        }
        private List<T> ToList(IOpenQuery<T> query)
        {
            Func<T, bool> predicate;
            if (query.WhereExpression == null)
            {
                predicate = new Func<T, bool>(m => { return true; });
            }
            else
            {
                predicate = this.GetFilter(query.WhereExpression);
            }
            var q = this.GetData().AsEnumerable().Where(predicate);
            var order = query.OrderExpression;
            while (order != null)
            {
                if (order.Descending)
                {
                    Func<T, string> keySelector = new Func<T, string>((target) => { return order.FieldName; });
                    q = q.OrderByDescending(keySelector);
                }
                else
                {
                    Func<T, string> keySelector = new Func<T, string>((target) => { return order.FieldName; });
                    q.OrderBy(keySelector);
                }
                order = order.Expression;
            }
            return q.ToList();
        }
        private PagedList<T> PageList(IOpenQuery<T> query)
        {
            var list = this.ToList(query);
            return new PagedList<T>(list.Skip(query.TakeExpression.Skip).Take(query.TakeExpression.Take).ToList(), query.TakeExpression.Skip / query.TakeExpression.Take + 1, query.TakeExpression.Take, list.Count);
        }
        private T Last(IOpenQuery<T> query)
        {
            return this.ToList(query).Last();
        }
        private T First(IOpenQuery<T> query)
        {
            return this.ToList(query).FirstOrDefault();
        }
        private int Count(IOpenQuery<T> query)
        {
            return this.ToList(query).Count;
        }
        #endregion
    }

    public class EntityJsonFileRepository<T> : BaseEntityJsonFileRepository<T> where T : EntityBase, new()
    {
        public EntityJsonFileRepository(string connectionString, string databaseName, Schema schema)
            : base(connectionString, databaseName, schema)
        { }

        #region Insert
        public override void Insert(T newData)
        {
            if (string.IsNullOrEmpty(newData.Id))
            {
                newData.Id = Guid.NewGuid().ToString("N");
            }
            newData.CreatedOn = DateTime.UtcNow;
            newData.UpdatedOn = DateTime.UtcNow;
            List<T> list = this.GetData();
            list.Add(newData);
            this.SetData(list);
        }

        #endregion

        #region Delete
        public override void Delete(string uuid)
        {
            if (string.IsNullOrEmpty(uuid))
            {
                return;
            }
            var list = this.GetData();
            var existedData = list.FirstOrDefault(m => m.Id == uuid);
            list.Remove(existedData);
            this.SetData(list);
        }
        public override void Delete(T oldData)
        {
            Delete(oldData.Id);
        }
        #endregion

        #region Update
        public override void Update(T newData, string uuid = "")
        {
            if (string.IsNullOrEmpty(uuid))
            {
                uuid = newData.Id;
            }

            if (string.IsNullOrEmpty(uuid))
            {
                return;
            }

            newData.UpdatedOn = DateTime.UtcNow;
            var list = this.GetData();
            if (list == null || list.Count < 1)
            {
                return;
            }
            var oldData = list.FirstOrDefault(m => m.Id == uuid);
            if (oldData == null)
            {
                return;
            }
            list.Remove(oldData);
            list.Add(newData);
            this.SetData(list);
        }
        #endregion

        #region Query

        #endregion
    }

    public class DynamicEntityJsonFileRepository<T> : BaseEntityJsonFileRepository<T> where T : DynamicEntity, new()
    {
        public DynamicEntityJsonFileRepository(string connectionString, string databaseName, Schema schema)
            : base(connectionString, databaseName, schema)
        { }

        #region Insert
        public override void Insert(T newData)
        {
            if (string.IsNullOrEmpty(newData.Id))
            {
                newData.Id = Guid.NewGuid().ToString("N");
            }
            newData.CreatedOn = DateTime.UtcNow;
            newData.UpdatedOn = DateTime.UtcNow;
            List<T> list = this.GetData();
            list.Add(newData);
            this.SetData(list);
        }

        #endregion

        #region Delete
        public override void Delete(string uuid)
        {
            if (string.IsNullOrEmpty(uuid))
            {
                return;
            }
            var list = this.GetData();
            var existedData = list.FirstOrDefault(m => m.Id == uuid);
            list.Remove(existedData);
            this.SetData(list);
        }
        public override void Delete(T oldData)
        {
            Delete(oldData.Id);
        }
        #endregion

        #region Update
        public override void Update(T newData, string uuid = "")
        {
            if (string.IsNullOrEmpty(uuid))
            {
                uuid = newData.Id;
            }

            if (string.IsNullOrEmpty(uuid))
            {
                return;
            }

            newData.UpdatedOn = DateTime.UtcNow;
            var list = this.GetData();
            if (list == null || list.Count < 1)
            {
                return;
            }
            var oldData = list.FirstOrDefault(m => m.Id == uuid);
            if (oldData == null)
            {
                return;
            }
            list.Remove(oldData);
            list.Add(newData);
            this.SetData(list);
        }
        #endregion

        #region Query

        #endregion
    }
}