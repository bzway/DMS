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
    public class Hidden : Input
    {
        public override string Name
        {
            get { return "Hidden"; }
        }

        public override string Type
        {
            get { return "hidden"; }
        }
        public override string Render(ISchema schema, IColumn column)
        {
            return base.RenderInput(column);
        }
    }
}
