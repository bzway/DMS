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
    public class CreateForm : ISchemaForm
    {
        #region ISchemaForm Members

        public string Generate(ISchema schema)
        {
            StringBuilder sb = new StringBuilder(string.Format(@"
@using Bzway.Data.Models;
@using Bzway.Data.Query;
@{{ var schema = (Bzway.Data.Models.Schema)ViewData[""Schema""];
    var allowedEdit = (bool)ViewData[""AllowedEdit""];
    var allowedView = (bool)ViewData[""AllowedView""];
    var parentUUID = ViewContext.RequestContext.AllRouteValues()[""parentUUID""];
    var parentFolder=ViewContext.RequestContext.AllRouteValues()[""parentFolder""];
}}
@using(Html.BeginForm(ViewContext.RequestContext.AllRouteValues()[""action""].ToString()
    , ViewContext.RequestContext.AllRouteValues()[""controller""].ToString()
    , ViewContext.RequestContext.AllRouteValues()
    , FormMethod.Post
    , new RouteValueDictionary(new {{ enctype = ""{0}"" }})))
{{
    <table>", FormHelper.Enctype(schema)));

            foreach (var item in schema.Columns.OrderBy(it => it.Order))
            {
                sb.Append(item.Render(schema, false));
            }

            sb.Append(@"
    @Html.Action(""Categories"", ViewContext.RequestContext.AllRouteValues())
    
    </table>
}");

            return sb.ToString();
        }
        #endregion
    }
}
