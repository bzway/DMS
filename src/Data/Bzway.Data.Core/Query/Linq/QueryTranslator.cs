
//using Bzway.Data;
//using Bzway.Data.Query;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;

//namespace QueryTranslator
//{

//    internal class QueryTranslator : System.Linq.Expressions.ExpressionVisitor
//    {
//        #region ctor
//        public string Table { get; set; }
//        public List<OpenFilter> Conditions { get; set; }
//        public List<OrderField> Sorts { get; set; }
//        Func<dynamic> Monitor;
//        public List<string> Selects { get; set; }

//        public int Top { get; internal set; }
//        public int Skip { get; internal set; }
//        public int TakeCount { get; internal set; }

//        internal QueryTranslator()
//        {
//        }

//        #endregion
//        internal string Translate(Expression expression, ISchemaRepository schema)
//        {
//            this.Table = schema.SchemaName;
//            this.Selects = new List<string>();
//            this.Conditions = new List<OpenFilter>();
//            this.Sorts = new List<OrderField>();

//            schema.QueryColumn().ToList().ForEach((e) => { this.Selects.Add(e.Name); });
//            this.Monitor = () => { return null; };

//            this.Visit(expression);
//            return string.Empty;
//        }

//        private static Expression StripQuotes(Expression e)
//        {
//            while (e.NodeType == ExpressionType.Quote)
//            {
//                e = ((UnaryExpression)e).Operand;
//            }
//            return e;
//        }

//        protected override Expression VisitMethodCall(MethodCallExpression m)
//        {
//            if (m.Method.DeclaringType != typeof(Queryable))
//            {

//                if (m.Method.DeclaringType == typeof(OpenEntity))
//                {
//                    this.Visit(m.Arguments[0]);
//                    LambdaExpression lambdaX = (LambdaExpression)StripQuotes(m.Arguments[1]);
//                    this.Visit(lambdaX.Body);
//                    return m;
//                }
//                throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));
//            }

//            switch (m.Method.Name)
//            {
//                case "Select":
//                    this.Visit(m.Arguments[0]);
//                    return m;
//                case "Where":
//                    this.Visit(m.Arguments[0]);
//                    LambdaExpression lambda = (LambdaExpression)StripQuotes(m.Arguments[1]);

//                    var x = new OpenFilter();
//                    this.Monitor = () =>
//                    {
//                        return x;
//                    };

//                    this.Visit(lambda.Body);
//                    this.Monitor = () =>
//                    {
//                        return null;
//                    };
//                    this.Conditions.Add(x);
//                    return m;
//                case "Take":
//                    this.Visit(m.Arguments[0]);
//                    var v = m.Arguments[1] as ConstantExpression;
//                    this.Top = (int)v.Value;
//                    return m;
//                case "OrderBy":
//                    this.Visit(m.Arguments[0]);
//                    LambdaExpression lambdaOrder = (LambdaExpression)StripQuotes(m.Arguments[1]);
//                    var y = new OrderField();
//                    this.Monitor = () =>
//                    {
//                        return y;
//                    };

//                    this.Visit(lambdaOrder.Body);
//                    this.Monitor = () =>
//                    {
//                        return null;
//                    };
//                    this.Sorts.Add(y);
//                    return m;
//                case "OrderByDescending":
//                    this.Visit(m.Arguments[0]);
//                    LambdaExpression lambdaDescendOrder = (LambdaExpression)StripQuotes(m.Arguments[1]);
//                    var z = new OrderField();
//                    this.Monitor = () =>
//                    {
//                        return z;
//                    };

//                    this.Visit(lambdaDescendOrder.Body);
//                    this.Monitor = () =>
//                    {
//                        return null;
//                    };
//                    this.Sorts.Add(z);
//                    return m;
//                case "First":
//                case "FirstOrDefault":
//                    this.Visit(m.Arguments[0]);
//                    this.Top = 1;
//                    return m;
//                case "get_Item":

//                    this.Visit(m.Arguments[0]);
//                    LambdaExpression lambdaget_Item = (LambdaExpression)StripQuotes(m.Arguments[1]);
//                    var r = new OpenFilter();
//                    this.Monitor = () =>
//                    {
//                        return r;
//                    };

//                    this.Visit(lambdaget_Item.Body);
//                    this.Monitor = () =>
//                    {
//                        return null;
//                    };
//                    this.Conditions.Add(r);
//                    return m;
//                default:
//                    throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));

//            }

//        }

//        protected override Expression VisitUnary(UnaryExpression u)
//        {
//            switch (u.NodeType)
//            {
//                case ExpressionType.Not:
//                    this.Conditions.Add(new OpenFilter() { ConditionType = ExpressionType.Not });
//                    this.Visit(u.Operand);
//                    break;
//                default:
//                    throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", u.NodeType));
//            }
//            return u;
//        }
//        protected override Expression VisitDynamic(DynamicExpression node)
//        {
//            return base.VisitDynamic(node);
//        }
//        protected override Expression VisitBinary(BinaryExpression b)
//        {
//            //where 

//            this.Visit(b.Left);
//            var o = this.Monitor();
//            o.ConditionType = b.NodeType;
//            //switch (b.NodeType)
//            //{
//            //    case ExpressionType.And:
//            //        //this.ClauseText += (" AND ");
//            //        break;
//            //    case ExpressionType.Or:
//            //        //this.ClauseText += (" OR");
//            //        break;
//            //    case ExpressionType.Equal:
//            //        this.condition.ConditionType = b.NodeType;
//            //        break;
//            //    case ExpressionType.NotEqual:
//            //        //this.ClauseText += (" <> ");
//            //        break;
//            //    case ExpressionType.LessThan:
//            //        //this.ClauseText += (" < ");
//            //        break;
//            //    case ExpressionType.LessThanOrEqual:
//            //        //this.ClauseText += (" <= ");
//            //        break;
//            //    case ExpressionType.GreaterThan:
//            //        //this.ClauseText += (" > ");
//            //        break;
//            //    case ExpressionType.GreaterThanOrEqual:
//            //        //this.ClauseText += (" >= ");
//            //        break;
//            //    default:
//            //        throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));
//            //}
//            this.Visit(b.Right);
//            //this.ClauseText += (") and ");
//            return b;
//        }
//        protected override Expression VisitIndex(IndexExpression node)
//        {
//            return base.VisitIndex(node);
//        }
//        protected override Expression VisitConstant(ConstantExpression c)
//        {
//            var o = this.Monitor();
//            if (o != null)
//            {
//                o.Value = c.Value.ToString();
//            }
//            return c;
//        }
//        protected override Expression VisitConditional(ConditionalExpression node)
//        {
//            return base.VisitConditional(node);
//        }
//        protected override Expression VisitLambda<T>(Expression<T> node)
//        {
//            return base.VisitLambda<T>(node);
//        }
//        protected override Expression VisitMember(MemberExpression node)
//        {
//            var o = this.Monitor();
//            o.Name = node.Member.Name;


//            return base.VisitMember(node);
//        }
//        protected Expression VisitMemberAccess(MemberExpression m)
//        {
//            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter)
//            {
//                this.Conditions.Add(new OpenFilter() { Name = m.Member.Name });
//                return m;
//            }
//            throw new NotSupportedException(string.Format("The member '{0}' is not supported", m.Member.Name));
//        }
//    }
//}
