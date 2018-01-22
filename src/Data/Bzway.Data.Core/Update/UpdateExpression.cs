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

namespace Bzway.Data.Core
{
  
    public class UpdateExpression  
    {
        public UpdateExpression Expression { get; private set; }
        public string FieldName { get; private set; }
         public object Value { get; private set; }
         public UpdateExpression(UpdateExpression Expression, string fieldName, object value)
        {
            this.Expression = Expression;
            this.FieldName = fieldName;
            this.Value = value; 
        }
    }

}