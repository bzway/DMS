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
    public class CLEditor : ControlBase
    {
        public override string Name
        {
            get { return "CLEditor"; }
        }

        protected override string RenderInput(IColumn column)
        {
            return string.Format(@"<textarea name=""{0}"" id=""{0}"" rows=""10"" cols=""100"">@Entity[""{0}""]</textarea> 
        <script language=""javascript"" type=""text/javascript"">$(function(){{$(""#{0}"").cleditor();}});</script>", column.Name);
        }
    }
}
