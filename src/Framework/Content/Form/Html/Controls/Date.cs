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
    public class Date : Input
    {
        public override string Name
        {
            get { return "Date"; }
        }
        public override string Type
        {
            get { return "text"; }
        }

        protected override string RenderInput(IColumn column)
        {
            string input = string.Format(@"<input class=""long"" id=""{0}"" name=""{0}"" type=""{1}"" value=""@(Entity.{0} ==null ? """" : Entity.{0}.ToLocalTime().ToShortDateString())"" {2}/>", column.Name, Type, OpenData.Framework.Common.Form.Html.ValidationExtensions.GetUnobtrusiveValidationAttributeString(column));
            return input + string.Format(@"<script language=""javascript"" type=""text/javascript"">$(function(){{$(""#{0}"").datepicker();}});</script>", column.Name);
        }
    }
}
