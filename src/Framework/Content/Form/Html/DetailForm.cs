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

namespace OpenData.Framework.Common.Form.Html
{
    public class DetailForm : ISchemaForm
    {
        public string Generate(ISchema schema)
        {
            StringBuilder sb = new StringBuilder("");
            sb.Append("@if (Entity != null)\r\n{ \r\n");
            sb.AppendFormat("<div>\r\n", schema.Name);
            sb.AppendFormat("\t<h3 class=\"title\" @ViewHelper.Edit(Entity,\"{0}\")>@Html.Raw(Entity.{0} ?? \"\")</h3>\r\n", schema.TitleColumn == null ? "Title" : schema.TitleColumn.Name);
            sb.Append("\t<div class=\"content\">\r\n");
            foreach (var column in schema.Columns.Where(it => !it.IsSystemField))
            {
                if (schema.TitleColumn != null && string.Compare(column.Name, schema.TitleColumn.Name, true) != 0)
                {
                    sb.AppendFormat("\t\t<div @ViewHelper.Edit(Entity,\"{0}\")>@Html.Raw(Entity.{0} ?? \"\")</div>\r\n", column.Name);
                }
            }
            sb.Append("\t</div>\r\n</div>");
            sb.Append("\r\n}");
            return sb.ToString();
        }
    }
}
