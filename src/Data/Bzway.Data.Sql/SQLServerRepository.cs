using Bzway.Data;
using Bzway.Data.Core;
using Bzway.Data.Core.OpenExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Bzway.Common.Collections;

namespace Bzway.Data.SQLServer
{
    public class BaseEntitySqlServerRepository<T> : IRepository<T> where T : new()
    {
        #region ctor
        readonly string connectionString;
        readonly string entityName;
        readonly Schema schema;
        public BaseEntitySqlServerRepository(string connectionString, Schema schema)
        {
            this.schema = schema;
            this.connectionString = connectionString;
            this.entityName = schema.Name;
        }
        #endregion
        #region Insert
        public virtual void Insert(T newData)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("INSERT INTO [{0}]", entityName));
            sb.AppendLine("(");
            foreach (var item in schema.AllColumns)
            {
                sb.AppendLine(string.Format("[{0}],", item.Name));
            }
            sb = sb.Remove(sb.Length - 3, 3);
            sb.AppendLine(")");
            sb.AppendLine("VALUES");
            sb.AppendLine("(");
            foreach (var item in schema.AllColumns)
            {
                var data = newData.TryGetValue(item.Name);
                if (data == null)
                {
                    sb.AppendLine("NULL,");
                    continue;
                }
                switch (item.ControlType)
                {
                    case "":
                        sb.AppendLine(string.Format("'{0}',", newData.TryGetValue(item.Name)));
                        break;
                    default:
                        sb.AppendLine(string.Format("'{0}',", newData.TryGetValue(item.Name)));
                        break;
                }
            }
            sb = sb.Remove(sb.Length - 3, 3);
            sb.AppendLine(")");
            SqlHelper.ExecuteNonQuery(this.connectionString, CommandType.Text, sb.ToString());
        }

        #endregion

        #region Delete
        public void Delete(T oldData)
        {
            if (oldData is DynamicEntity)
            {
                var t = (DynamicEntity)Convert.ChangeType(oldData, typeof(DynamicEntity));
                Delete(t.Id);
                return;
            }
            var uuid = oldData.TryGetValue("uuid");
            if (uuid != null)
            {
                Delete(uuid.ToString());
                return;
            }
        }
        public void Delete(string uuid)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("DELETE FROM [{0}]", entityName));
            sb.AppendLine(string.Format("WHERE Id = '{0}'", uuid));
            SqlHelper.ExecuteNonQuery(this.connectionString, CommandType.Text, sb.ToString());
        }

        public void Delete(IWhereExpression expression)
        {
            return;
        }

        #endregion

        #region Update
        public virtual void Update(T newData, string id = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("UPDATE	[{0}] SET", entityName));

            foreach (var item in schema.AllColumns.Where(m => !m.IsSystemField))
            {
                var data = newData.TryGetValue(item.Name);
                if (data == null)
                {
                    sb.AppendLine(string.Format("[{0}] = NULL,", item.Name));
                }
                else
                {
                    sb.AppendLine(string.Format("[{0}] = '{1}',", item.Name, newData.TryGetValue(item.Name)));
                }
            }
            sb.AppendLine(string.Format("UpdatedOn ='{0}',", newData.TryGetValue("UpdatedOn")));
            sb.AppendLine(string.Format("UpdatedBy ='{0}',", newData.TryGetValue("UpdatedBy")));
            sb.AppendLine(string.Format("UserKey ='{0}',", newData.TryGetValue("UserKey")));
            sb.AppendLine(string.Format("Status ='{0}'", newData.TryGetValue("Status")));
            sb.AppendLine(string.Format("WHERE Id = '{0}'", id));
            SqlHelper.ExecuteNonQuery(this.connectionString, CommandType.Text, sb.ToString());
        }
        public IUpdate<T> Update(IWhereExpression where)
        {
            return new OpenUpdate<T>(this, where);
        }
        public bool Execute(IUpdate<T> update)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Update ");
            sb.AppendLine(string.Format("Update [{0}] set", this.entityName));



            var updateExpression = update.UpdateExpression;
            while (updateExpression != null)
            {
                sb.AppendLine(string.Format("[{0}] = '{1}'", updateExpression.FieldName, updateExpression.Value));
                updateExpression = updateExpression.Expression;
            }
            sb.AppendLine("Where 1=1");

            sb.AppendLine(this.GetFilter(update.WhereExpression));
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
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT");
            if (query.SelectExpression == null || query.SelectExpression.Fields.Length < 1)
            {
                sb.AppendLine("*");
            }
            else
            {
                sb.AppendLine(string.Join(",", query.SelectExpression.Fields));
            }
            sb.AppendLine(string.Format("From [{0}]", this.entityName));
            sb.AppendLine("Where 1=1");
            var where = this.GetFilter(query.WhereExpression);
            if (!string.IsNullOrEmpty(where))
            {
                sb.AppendLine("and " + where);
            }
            sb.AppendLine("order by ID");
            var order = query.OrderExpression;
            while (order != null)
            {
                if (order.Descending)
                {
                    sb.AppendLine(string.Format(",{0} desc", order.FieldName));
                }
                else
                {
                    sb.AppendLine(string.Format(",{0}", order.FieldName));
                }
                order = order.Expression;
            }
            List<T> list = new List<T>();
            using (var reader = SqlHelper.ExecuteReader(this.connectionString, CommandType.Text, sb.ToString()))
            {

                while (reader.Read())
                {
                    var entity = new T();
                    foreach (var item in this.schema.AllColumns)
                    {
                        entity.TrySetValue(item.Name, reader[item.Name]);
                    }
                    list.Add(entity);
                }
            }
            return list;
        }

        private string GetFilter(IWhereExpression query)
        {
            StringBuilder filter = new StringBuilder();

            if (query != null)
            {
                if (query is WhereExpression)
                {
                    filter.AppendLine(GetAndFilter((WhereExpression)query));
                }
                else
                {
                    filter.AppendLine(GetOrFilter((OrAlsoExpression)query));
                }
            }
            return filter.ToString();
        }
        private string GetAndFilter(WhereExpression where)
        {
            StringBuilder filter = new StringBuilder();
            switch (where.CompareType)
            {
                case CompareType.Equal:
                    filter.AppendLine(string.Format("{0} = '{1}'", where.FieldName, where.Value));
                    break;
                case CompareType.NotEqual:
                    filter.AppendLine(string.Format("{0} = '{1}'", where.FieldName, where.Value));
                    break;
                case CompareType.Like:
                    filter.AppendLine(string.Format("{0} = '{1}'", where.FieldName, where.Value));
                    break;
                case CompareType.GreaterThan:
                    filter.AppendLine(string.Format("{0} = '{1}'", where.FieldName, where.Value));
                    break;
                case CompareType.GreaterThanOrEqual:
                    filter.AppendLine(string.Format("{0} = '{1}'", where.FieldName, where.Value));
                    break;
                case CompareType.LessThan:
                    filter.AppendLine(string.Format("{0} = '{1}'", where.FieldName, where.Value));
                    break;
                case CompareType.LessThanOrEqual:
                    filter.AppendLine(string.Format("{0} = '{1}'", where.FieldName, where.Value));
                    break;
                case CompareType.Startwith:
                    filter.AppendLine(string.Format("{0} = '{1}'", where.FieldName, where.Value));
                    break;
                case CompareType.EndWith:
                    filter.AppendLine(string.Format("{0} = '{1}'", where.FieldName, where.Value));
                    break;
                case CompareType.Contains:
                    filter.AppendLine(string.Format("{0} = '{1}'", where.FieldName, where.Value));
                    break;
                case CompareType.NoLike:
                    break;
                default:
                    break;
            }
            if (where.Expression == null)
            {
                return filter.ToString();
            }
            if (where.Expression is OrAlsoExpression)
            {
                return filter.AppendLine(string.Format("OR {0}", GetOrFilter((OrAlsoExpression)where.Expression))).ToString();
            }
            else
            {
                return filter.AppendLine(string.Format("And {0}", GetAndFilter((WhereExpression)where.Expression))).ToString();
            }
        }

        private string GetOrFilter(OrAlsoExpression or)
        {
            StringBuilder filterLeft = new StringBuilder();
            if (or.Left is WhereExpression)
            {
                filterLeft.AppendLine(GetAndFilter((WhereExpression)or.Left));
            }
            else
            {
                filterLeft.AppendLine(GetOrFilter((OrAlsoExpression)or.Left));
            }
            StringBuilder filterRight = new StringBuilder();
            if (or.Right is WhereExpression)
            {
                filterRight.AppendLine(GetAndFilter((WhereExpression)or.Right));
            }
            else
            {
                filterRight.AppendLine(GetOrFilter((OrAlsoExpression)or.Right));
            }
            return string.Format("({0}) OR ({1})", filterLeft, filterRight);
        }

        private PagedList<T> PageList(IOpenQuery<T> query)
        {
            var list = this.ToList(query);
            return new PagedList<T>(list, query.TakeExpression.Skip / query.TakeExpression.Take + 1, query.TakeExpression.Take, list.Count);
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
    public class EntitySqlServerRepository<T> : BaseEntitySqlServerRepository<T> where T : EntityBase, new()
    {
        public EntitySqlServerRepository(string connectionString, Schema schema)
            : base(connectionString, schema)
        {

        }
        public override void Insert(T newData)
        {
            base.Insert(newData);
        }
    }
    public class DynamicEntitySqlServerRepository<T> : BaseEntitySqlServerRepository<T> where T : DynamicEntity, new()
    {
        public DynamicEntitySqlServerRepository(string connectionString, Schema schema)
            : base(connectionString, schema)
        {

        }
        public override void Insert(T newData)
        {

        }
    }
}