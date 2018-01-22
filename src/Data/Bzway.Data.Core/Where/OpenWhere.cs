
using Bzway.Data.Core.OpenExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Bzway.Data.Core
{
    public class OpenWhere<T> : IWhere<T>
    {
        public IWhereExpression Expression
        {
            get;
            private set;
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

        public IWhere<T> Where<Key>(Expression<Func<T, Key>> keySelector, object value, CompareType type = CompareType.Equal)
        {
            string fieldName = TryGetName(keySelector.Body);
            if (!string.IsNullOrEmpty(fieldName))
            {
                this.Expression = new WhereExpression(this.Expression, fieldName, value, CompareType.GreaterThan);
            }
            return this;
        }

        public IWhere<T> Where(string fieldName, object value, CompareType type = CompareType.Equal)
        {
            this.Expression = new WhereExpression(this.Expression, fieldName, value, type);
            return this;
        }
        public IWhere<T> Or(IWhere<T> query)
        {

            this.Expression = new OrAlsoExpression(this.Expression, query.Expression);
            return this;
        }
    }
}