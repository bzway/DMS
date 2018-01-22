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
    public class SelectOpenExpression
    {
        public SelectOpenExpression Expression { get; set; }
        public SelectOpenExpression(SelectOpenExpression Expression, string[] fields)
        {
            this.Expression = Expression;
            this.Fields = fields;
        }
        public string[] Fields { get; private set; }
    }
}
