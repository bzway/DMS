#region License
// 
// Copyright (c) 2013, Bzway team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bzway.Data.Core.OpenExpressions
{
    public interface IWhereExpression
    { }
    public class WhereExpression : IWhereExpression
    {
        public IWhereExpression Expression { get; private set; }
        public string FieldName { get; private set; }
        public CompareType CompareType { get; private set; }
        public object Value { get; private set; }
        public WhereExpression(IWhereExpression Expression, string fieldName, object value, CompareType type)
        {
            this.Expression = Expression;
            this.FieldName = fieldName;
            this.Value = value;
            this.CompareType = type;
        }
    }

}