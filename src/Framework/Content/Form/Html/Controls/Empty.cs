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
    public class Empty : IControl
    {
        public string Name
        {
            get
            {
                return "Empty";
            }
        }

        public string Render(ISchema schema, IColumn column)
        {
            return string.Format(@"<input type=""hidden"" name=""{0}"" value=""@(Entity.{0} ?? """")""/>", column.Name);
        }

        public bool IsFile
        {
            get { return false; }
        }

        public string GetValue(object oldValue, string newValue)
        {
            return newValue;
        }


        public bool HasDataSource
        {
            get { return false; }
        }
    }
}
