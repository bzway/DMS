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

namespace OpenData.Framework.Common.Css.Meta
{
    public class PositionShorthandRule : PositionDescriminationRule
    {
        public static readonly string[] Positions = "top right bottom left".Split(' ');

        public PositionShorthandRule()
            : base(
                "top right bottom left",
                "top left,right bottom",
                "top,bottom left,right",
                "top,right,bottom,left")
        {
        }
    }
}
