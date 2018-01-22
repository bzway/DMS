using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bzway.Data.Core
{

    public class OpenQueryable<T> : IQueryable<T>, IOrderedQueryable<T>
    {
        public OpenQueryable(Schema schema)
        {
            this.Provider = new OpenQueryProvider(schema);
            this.Expression = Expression.Constant(this);
        }
        public OpenQueryable(IQueryProvider queryProvider, Expression expression)
        {
            this.Provider = queryProvider;
            this.Expression = expression;
        }
        public IEnumerator<T> GetEnumerator()
        {
            var test = this.Provider.Execute<IEnumerable<T>>(this.Expression) as IEnumerable<T>;

            return test.GetEnumerator();
        }


        public Type ElementType
        {
            get { return typeof(T); }
            private set { ElementType = value; }
        }

        public Expression Expression
        {
            get;
            private set;
        }

        public IQueryProvider Provider
        {
            get;
            private set;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (this.Provider.Execute(this.Expression) as IEnumerable).GetEnumerator();

        }
    }

}