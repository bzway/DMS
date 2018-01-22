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
using System.Web;

namespace OpenData.Framework.Common.Form.Html.Controls
{
    public class InputNumber : ControlBase
    {
        #region IControl Members

        public override string Name
        {
            get
            {
                return "Number";
            }
        }
        protected override string RenderInput(IColumn column)
        {
            
            return string.Format(@"<input id=""{0}"" name=""{0}"" type=""{1}"" value=""@(Entity.{0} ?? """")"" {2} value-type=""{3}""/>", column.Name, Name, OpenData.Framework.Common.Form.Html.ValidationExtensions.GetUnobtrusiveValidationAttributeString(column),"float");
        }


        #endregion
    }
}
