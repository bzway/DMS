using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;

namespace Bzway.Database.Core
{
    public interface IUpdate<T>
    {
        ICollection<KeyValuePair<string, object>> Updates { get; }
        IUpdate<T> Update<Key>(Expression<Func<T, Key>> keySelector, object value);
        IUpdate<T> Update(string fieldName, object value);
    }


}
