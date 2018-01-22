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
    public class File : Input
    {
        public override string Name
        {
            get { return "File"; }
        }

        public override string Type
        {
            get { return "file"; }
        }
        public override bool IsFile
        {
            get
            {
                return true;
            }
        }
        protected override string RenderInput(IColumn column)
        {
            return string.Format(@"<input id=""{0}"" name=""{0}"" type=""{1}"" value=""@(Entity.{0} ?? """")""  data-value=""@(Entity.{0} ?? """")"" class='filestyle' {2}/>", column.Name, Type, OpenData.Framework.Common.Form.Html.ValidationExtensions.GetUnobtrusiveValidationAttributeString(column));
        }
    }
}
