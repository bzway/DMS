
using Bzway.Common.Collections;
using Bzway.Data.Core.OpenExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Bzway.Data.Core
{
    public class OpenQuery<T> : IOpenQuery<T>
    {
        #region Ctor
        public IRepository<T> Repository
        {
            get;
            private set;
        }

        public SelectOpenExpression SelectExpression
        {
            get;
            private set;
        }

        public IWhereExpression WhereExpression
        {
            get;
            private set;
        }

        public CallExpression CallExpression
        {
            get;
            private set;
        }

        public OrderOpenExpression OrderExpression
        {
            get;
            private set;
        }


        public TakeOpenExpression TakeExpression
        {
            get;
            private set;
        }
        public OpenQuery(IRepository<T> repository, params string[] fields)
        {
            if (fields.Count() > 0)
            {
                this.SelectExpression = new SelectOpenExpression(this.SelectExpression, fields);
            }
            this.Repository = repository;
        }

        #endregion

        #region Order
        public IOpenQuery<T> OrderBy(string fieldName, bool descending = false)
        {
            this.OrderExpression = new OrderOpenExpression(this.OrderExpression, fieldName, descending);
            return this;
        }
        public IOpenQuery<T> OrderBy<Key>(Expression<Func<T, Key>> keySelector, bool descending = false)
        {
            string fieldName = TryGetName(keySelector.Body);
            if (!string.IsNullOrEmpty(fieldName))
            {
                this.OrderExpression = new OrderOpenExpression(this.OrderExpression, fieldName, descending);
            }
            return this;
        }
        #endregion

        #region Where

        string TryGetName(Expression expression)
        {
            try
            {
                if (expression is MemberExpression)
                {
                    return ((MemberExpression)expression).Member.Name;
                }
                else
                {
                    return ((ConstantExpression)((MethodCallExpression)expression).Arguments[0]).Value.ToString();
                }
            }
            catch
            {
                return string.Empty;
            }
        }
        public IOpenQuery<T> Where<Key>(Expression<Func<T, Key>> keySelector, object value, CompareType type = CompareType.Equal)
        {
            string fieldName = TryGetName(keySelector.Body);
            if (!string.IsNullOrEmpty(fieldName))
            {
                this.WhereExpression = new WhereExpression(this.WhereExpression, fieldName, value, type);
            }
            return this;
        }
        public IOpenQuery<T> Where(Expression<Func<T, bool>> where)
        {
            var fieldName = ((MemberExpression)((BinaryExpression)where.Body).Left).Member.Name;
            var value = ((ConstantExpression)((BinaryExpression)where.Body).Right).Value;
            CompareType type;
            switch ((((BinaryExpression)where.Body).NodeType))
            {
                case ExpressionType.Equal:
                    type = CompareType.Equal; break;
                case ExpressionType.GreaterThanOrEqual:
                    type = CompareType.GreaterThanOrEqual;
                    break;
                case ExpressionType.GreaterThan:
                    type = CompareType.GreaterThan;
                    break;
                default:
                    type = CompareType.Equal;
                    break;

            }
            if (!string.IsNullOrEmpty(fieldName))
            {
                this.WhereExpression = new WhereExpression(this.WhereExpression, fieldName, value, type);
            }
            return this;
        }

        public IOpenQuery<T> Where(string fieldName, object value, CompareType type)
        {
            this.WhereExpression = new WhereExpression(this.WhereExpression, fieldName, value, type);
            return this;
        }

        public IOpenQuery<T> Or(IWhere<T> query)
        {
            this.WhereExpression = new OrAlsoExpression(this.WhereExpression, query.Expression);
            return this;
        }
        #endregion

        #region Execute

        public int Count()
        {
            this.CallExpression = new CallExpression(CallType.Count);
            return (int)this.Repository.Execute(this);
        }

        public T First()
        {
            this.CallExpression = new CallExpression(CallType.First);
            var result = (T)this.Repository.Execute(this);
            return result;
        }
        public T Last()
        {
            this.CallExpression = new CallExpression(CallType.Last);
            return (T)this.Repository.Execute(this);
        }

        public PagedList<T> ToPageList(int index, int pageSize = 10)
        {
            this.CallExpression = new CallExpression(CallType.PageList);
            this.TakeExpression = new TakeOpenExpression(pageSize * (index - 1), pageSize);
            return (PagedList<T>)this.Repository.Execute(this);
        }
        public IEnumerable<T> ToList(int skip = 0, int take = 0)
        {
            this.CallExpression = new CallExpression(CallType.ToList);
            this.TakeExpression = new TakeOpenExpression(skip, take);
            return (IEnumerable<T>)this.Repository.Execute(this);
        }

        #endregion
    }
}