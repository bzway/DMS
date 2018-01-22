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
    public interface IControl
    {
        string Name { get; }

        string Render(ISchema schema, IColumn column);

        bool IsFile { get; }

        bool HasDataSource { get; }

        string GetValue(object oldValue, string newValue);
    }
}
