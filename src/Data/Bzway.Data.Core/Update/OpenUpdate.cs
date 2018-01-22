 
using Bzway.Data.Core.OpenExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Bzway.Data.Core
{
    public class OpenUpdate<T> : IUpdate<T>
    {
        public IWhereExpression WhereExpression
        {
            get;
            private set;
        }


        public UpdateExpression UpdateExpression
        {
            get;
            private set;
        }

        public IRepository<T> Repository
        {
            get;
            private set;
        }
        public OpenUpdate(IRepository<T> repository, IWhereExpression where)
        {
            this.Repository = repository;
            this.WhereExpression = where;
        }
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
        public IUpdate<T> Update<Key>(Expression<Func<T, Key>> keySelector, object value)
        {
            string fieldName = TryGetName(keySelector.Body);
            if (!string.IsNullOrEmpty(fieldName))
            {
                this.UpdateExpression = new UpdateExpression(this.UpdateExpression, fieldName, value);
            }
            return this;

        }
        public IUpdate<T> Update(string fieldName, object value)
        {
            this.UpdateExpression = new UpdateExpression(this.UpdateExpression, fieldName, value);
            return this;
        }
        public bool Update()
        {
            return this.Repository.Execute(this);
        }


    }
}