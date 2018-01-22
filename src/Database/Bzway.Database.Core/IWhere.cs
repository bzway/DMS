using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;

namespace Bzway.Database.Core
{
    public interface IWhere<T>
    {
        //IWhere<T> WhereExpression { get; }
        //string FieldName { get; }
        //CompareType CompareType { get; }
        //object Value { get; }
    }

    public class WhereExpression<T> : IWhere<T>
    {
        public string FieldName { get; private set; }

        public CompareType CompareType { get; private set; }

        public object Value { get; private set; }
        public IWhere<T> Next { get; private set; }

        public WhereExpression(string field, object value, CompareType type, IWhere<T> where)
        {
            this.FieldName = field;
            this.CompareType = type;
            this.Value = value;
            this.Next = where;
        }
    }
    public class OrExpression<T> : IWhere<T>
    {
        public OrExpression(IWhere<T> left, IWhere<T> right)
        {
            this.Left = left;
            this.Right = right;
        }
        public IWhere<T> Left { get; private set; }
        public IWhere<T> Right { get; private set; }
    }
}
