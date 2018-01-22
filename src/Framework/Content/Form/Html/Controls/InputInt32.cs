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
    public class InputInt32 : ControlBase
    {
        #region IControl Members

        public override string Name
        {
            get
            {
                return "Int32";
            }
        }
        protected override string RenderInput(IColumn column)
        {
            return string.Format(@"<input class=""long"" id=""{0}"" name=""{0}"" type=""text"" value=""@(Entity.{0} ?? """")"" {1}/>", column.Name, OpenData.Framework.Common.Form.Html.ValidationExtensions.GetUnobtrusiveValidationAttributeString(column));
        }


        #endregion
    }
}
