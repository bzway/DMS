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
    public class RadioList : ControlBase
    {
        public override string Name
        {
            get { return "RadioList"; }
        }
        public override bool HasDataSource
        {
            get
            {
                return true;
            }
        }
        protected override string RenderInput(IColumn column)
        {
            StringBuilder sb = new StringBuilder(string.Format(@"@{{ var radioDefaultValue_{0} = @""{1}"";}}", column.Name, column.DefaultValue.EscapeQuote()));
            if (!string.IsNullOrEmpty(column.SelectionFolder))
            {
                sb.AppendFormat(@"
                        @{{
                            var textFolder_{0} = new TextFolder(Repository.Current, ""{1}"");
                            var query_{0} = textFolder_{0}.CreateQuery().DefaultOrder();
                        }}
                        <ul class=""radio-list"">
                        @foreach (var item in query_{0})
                        {{                            
                         <li>
                           <input id=""{0}"" name=""{0}"" type=""radio"" value=""@item.Id""  @((Entity.{0} != null && Entity.{0}.ToString().ToLower() == @item.Id.ToLower()) || (Entity.{0} == null && radioDefaultValue_{0}.ToLower() == @item.Id.ToLower()) ? ""checked"" : """")/><label for=""{0}""  class=""inline"">@item.GetSummary()</label>
                         </li>
                        }}
                        </ul>
                        ", column.Name, column.SelectionFolder);
            }
            else
            {

                if (column.SelectionItems != null)
                {
                    var index = 0;
                    sb.Append(@"<ul class=""radio-list"">");
                    foreach (var item in column.SelectionItems)
                    {
                        var id = column.Name + "_" + index.ToString();
                        index++;
                        sb.AppendFormat(@"
<li>
<input id=""{0}"" name=""{1}"" type=""radio"" value=""@(@""{2}"")""  @((Entity.{1} != null && Entity.{1}.ToString().ToLower() == @""{2}"".ToLower()) || (Entity.{1} == null && radioDefaultValue_{1}.ToLower() == @""{2}"".ToLower()) ? ""checked"" : """")/><label for=""{0}"" class=""inline"">{3}</label>
</li>"
                            , id, column.Name, item.Value.EscapeQuote(), item.Text);
                    }
                    sb.Append("</ul>");
                }

            }
            return sb.ToString();
        }
    }
}
