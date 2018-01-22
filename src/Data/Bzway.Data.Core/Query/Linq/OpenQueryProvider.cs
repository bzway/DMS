
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
    public class OpenQueryProvider : IQueryProvider
    {
        Schema schema;
        //IColumnRepository schema;
        public OpenQueryProvider(Schema schema)
        {
            this.schema = schema;
        }


        public IQueryable CreateQuery(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            Type elementType = expression.Type;
            IQueryable result;
            try
            {
                Type type = typeof(OpenQueryable<>).MakeGenericType(new Type[]
				{
					elementType
				});
                result = (IQueryable)Activator.CreateInstance(type, new object[]
				{
					this,
					expression
				});
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
            return result;
        }

        public TResult Execute<TResult>(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            if (!typeof(TResult).IsAssignableFrom(expression.Type))
            {
                throw new ArgumentException("Argument expression is not valid.");
            }
            object obj = this.Execute(expression);
            if (obj == null)
            {
                return default(TResult);
            }

            return (TResult)((object)obj);
        }
        public virtual object Execute(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            //var translator = new QueryTranslator.QueryTranslator();
            //var sql = translator.Translate(expression, this.schema);
            //IMongoClient client = new MongoClient();
            //var db = client.GetDatabase("test");
            //var collection = db.GetCollection<BsonDocument>(schema.Name);

            //FilterDefinition<BsonDocument> filter = null;
            //foreach (var item in translator.Conditions)
            //{
            //    switch (item.ConditionType)
            //    {
            //        case ExpressionType.Equal:
            //            if (filter == null)
            //            {
            //                filter = Builders<BsonDocument>.Filter.Eq(item.Name, item.Value);
            //            }
            //            else
            //            {
            //                filter &= Builders<BsonDocument>.Filter.Eq(item.Name, item.Value);
            //            }
            //            break;
            //        case ExpressionType.NotEqual:
            //            if (filter == null)
            //            {
            //                filter = Builders<BsonDocument>.Filter.Ne(item.Name, item.Value);
            //            }
            //            else
            //            {
            //                filter &= Builders<BsonDocument>.Filter.Ne(item.Name, item.Value);
            //            }
            //            break;
            //        default:
            //            throw new Exception(item.ConditionType.ToString());
            //    }
            //}


            //var sort = Builders<BsonDocument>.Sort.Ascending("Name").Ascending("_id");
            //var result = collection.Find(filter).Sort(sort).ToListAsync();

            //List<OpenEntity> list = new List<OpenEntity>();

            //foreach (var doc in result.Result)
            //{

            //    var entity = new OpenEntity { UUID = sql };


            //    foreach (var item in this.schema.Query().ToList())
            //    {
            //        if (doc.Contains(item.Name))
            //        {
            //            entity[item.Name] = doc[item.Name];
            //        }
            //    }
            //    list.Add(entity);
            //}
            //if (translator.Top == 1)
            //{
            //    return list.FirstOrDefault();
            //}

            //return list;
            return "";
        }



        public IQueryable<T> CreateQuery<T>(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            if (!typeof(IQueryable<T>).IsAssignableFrom(expression.Type))
            {
                throw new ArgumentOutOfRangeException("expression");
            }

            return new OpenQueryable<T>(this, expression);
        }

    }
}