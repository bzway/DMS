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

namespace OpenData.Framework.Common.Form.Html.Controls
{
    public class TextArea : ControlBase
    {
        public override string Name
        {
            get { return "TextArea"; }
        }

        protected override string RenderInput(IColumn column)
        {
            return string.Format(@"<textarea class=""extra-large"" name=""{0}"" {1}>@(Entity.{0} ?? """")</textarea> ", column.Name, OpenData.Framework.Common.Form.Html.ValidationExtensions.GetUnobtrusiveValidationAttributeString(column));
        }
    }
}
