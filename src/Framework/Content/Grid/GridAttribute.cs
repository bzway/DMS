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

namespace OpenData.Framework.Common.Grid
{
    public class GridAttribute : Attribute
    {
        public bool Checkable { get; set; }
        public string IdProperty { get; set; }
        public string ItemDetailView { get; set; }
        public Type CheckVisible { get; set; }
    }
}
