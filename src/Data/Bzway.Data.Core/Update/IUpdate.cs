 
using Bzway.Data.Core.OpenExpressions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
namespace Bzway.Data.Core
{
    public interface IUpdate<T>
    {
        UpdateExpression UpdateExpression { get; }
        IWhereExpression WhereExpression { get; }
        IRepository<T> Repository { get; }
        IUpdate<T> Update<Key>(Expression<Func<T, Key>> keySelector, object value);
        IUpdate<T> Update(string fieldName, object value);
        bool Update();
    }
}