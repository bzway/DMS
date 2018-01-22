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
    public class OrderOpenExpression
    {
        public OrderOpenExpression(OrderOpenExpression expression, string fieldName, bool descending)
        {
            this.Expression = expression;
            this.Descending = descending;
            this.FieldName = fieldName;
        }
        public OrderOpenExpression Expression { get; private set; }
        public string FieldName { get; private set; }
        public bool Descending { get; private set; }
    }
}
