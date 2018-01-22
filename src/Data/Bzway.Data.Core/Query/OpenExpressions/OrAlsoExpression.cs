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
    public class OrAlsoExpression : IWhereExpression
    {
        public OrAlsoExpression(IWhereExpression left, IWhereExpression right)
        {
            this.Left = left;
            this.Right = right;
        }
        public IWhereExpression Left { get; private set; }
        public IWhereExpression Right { get; private set; }
    }
}
