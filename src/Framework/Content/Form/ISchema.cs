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

namespace OpenData.Framework.Common.Form
{
    public interface ISchema
    {
        string Name { get; set; }
        bool IsTreeStyle { get; set; }
        IEnumerable<IColumn> Columns { get; }
        IColumn this[string name] { get; }

        IColumn TitleColumn { get; }
    }
}
