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
    public class CallExpression
    {
        public CallExpression(
            //SelectOpenExpression select, TakeOpenExpression take, IWhereExpression where, OrderOpenExpression order
           CallType type
            )
        {
            //this.Select = select;
            //this.Take = take;
            //this.Where = where;
            //this.Order = order;
            this.CallType = type;
        }
        //public SelectOpenExpression Select { get; set; }

        //public IWhereExpression Where { get; set; }

        //public OrderOpenExpression Order { get; set; }

        //public TakeOpenExpression Take { get; set; }

        public CallType CallType { get; set; }
    }
    public class TakeOpenExpression
    {
        public TakeOpenExpression(int Skip, int Take)
        {
            this.Skip = Skip;
            this.Take = Take;
        }
        public int Skip { get; set; }

        public int Take { get; set; }
    }
}
