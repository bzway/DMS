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
using OpenData.Extensions;
using OpenData.Framework.Common;


namespace OpenData.Framework.Common.Form.Html
{
    public class GridForm : ISchemaForm
    {
        string Table = @"
@model Bzway.Web.Web.Areas.Contents.Models.TextContentGrid
@using Bzway.Data.Query
@using Bzway.Data.Models
@using Bzway.Web.Web.Areas.Contents.Controllers
@{{
    var schema = (Bzway.Data.Models.Schema)ViewData[""Schema""];
    var folder = (Bzway.Data.Models.TextFolder)ViewData[""Folder""];
    var routes = ViewContext.RequestContext.AllRouteValues();    
    var allowedEdit = (bool)ViewData[""AllowedEdit""];
    var allowedView = (bool)ViewData[""AllowedView""];
    var parentUUID = ViewContext.RequestContext.GetRequestValue(""parentUUID"") ?? """"; 
    var childFolders = Entity.ChildFolders==null? new TextFolder[0]:Entity.ChildFolders.ToArray();

}}

<div class=""common-table fixed"">
 <div class=""thead"">
    <table>
        <thead>
            <tr>
                <th class=""checkbox mutiple"">
                    <div>
                        <input type=""checkbox"" class="" select-all"" />
                        @Html.IconImage(""arrow"")
                        <ul class=""hide"">
                            <li>Select:</li>
                            <li class=""all""><a href=""javascript:;"">All Elements</a></li>
                            <li class=""docs""><a href=""javascript:;"">Only Documents</a></li>
                            @if (ViewBag.FolderPermission)
                            {{
                                <li class=""folders""><a href=""javascript:;"">Only Folders</a></li>
                            }}
                            <li class=""none""><a href=""javascript:;"">None</a></li>
                        </ul>
                    </div>
                </th>
                {0}
                @if (folder.EmbeddedFolders != null)
                {{
                    foreach (var s in folder.EmbeddedFolders)
                    {{
                    <th>@Bzway.Data.Models.IPersistableExtensions.AsActual(new TextFolder(Repository.Current, s)).FriendlyText
                    </th>
                    }}
                }}
                @if (Repository.Current.EnableWorkflow && folder.EnabledWorkflow)
                {{
                    <th class=""action"">
                        @(""Workflow"".Localize())
                    </th>
                }}
                               
                @if (schema.IsTreeStyle)
                {{
                    <th class=""action"">
                    </th> 
                }}
            </tr>
        </thead>
    </table>
</div>
<div class=""tbody"">
    <table>
        <tbody>
        @if (childFolders.Length == 0 && ViewBag.PagedList.TotalItemCount == 0)
        {{
            <tr class=""empty"">
                <td>
                    @(""Empty"".Localize())
                </td>
            </tr>
        }}
        else{{
            foreach (dynamic item in childFolders)
                {{
                    <tr class=""foldertr @((item.Hidden == true)? ""hidden-folder"":"""")"">
                        <td class=""checkbox mutiple undraggable"">
                            @if (ViewBag.FolderPermission)
                            {{
                                <input type=""checkbox"" name=""select"" class=""select folder"" id=""@item.FullName"" value=""@item.FullName"" data-id-property=""Id"" />
                            }}
                        </td>
                        <td>
                            @if (!string.IsNullOrEmpty(item.SchemaName))
                            {{
                                <a href=""@this.Url.Action(""Index"", ViewContext.RequestContext.AllRouteValues().Merge(""FolderName"", (object)(item.FullName)).Merge(""FullName"", (object)(item.FullName)))"" >
                                   @Html.IconImage(""folder"") @Bzway.Data.Models.IPersistableExtensions.AsActual(item).FriendlyText</a>
                            }}
                            else
                            {{
                                <a href=""@this.Url.Action(""Index"", ViewContext.RequestContext.AllRouteValues().Merge(""controller"", ""TextFolder"").Merge(""FolderName"", (object)(item.FullName)).Merge(""FullName"", (object)(item.FullName)))"" >
                                   @Html.IconImage(""folder"") @Bzway.Data.Models.IPersistableExtensions.AsActual(item).FriendlyText</a>
                            }}
                        </td>
                        <td colspan=""{2}"">
                        </td>
                        @if (Repository.Current.EnableWorkflow && folder.EnabledWorkflow)
                        {{
                            <td colspan=""1"">
                            </td>
                        }}
                        @if (folder.EmbeddedFolders != null)
                        {{
                            <td colspan=""@folder.EmbeddedFolders.Count()"">
                            </td>
                        }}
                      
                        @if (schema.IsTreeStyle)
                        {{
                            <td>
                            </td>
                        }}
                    </tr>
                }}
            @AddTreeItems(ViewBag.PagedList, schema, allowedEdit, folder, """")
        }}
            
        </tbody>
    </table>
</div>
</div>
@helper AddTreeItems(IEnumerable<TextContent> items, Schema schema, bool allowedEdit, TextFolder folder, string parentChain)
    {{
        var isRoot = string.IsNullOrEmpty(parentChain);
        //column datasource
        {3}
        if (Repository.Current.EnableWorkflow == true)
        {{
            items = Bzway.Data.Services.ServiceFactory.WorkflowManager.GetPendWorkflowItemForContents(Repository.Current, items.ToArray(), User.Identity.Name);
        }}
        foreach (dynamic item in items)
        {{
            var workflowItem = item._WorkflowItem_;
            var hasworkflowItem = workflowItem != null;
            var availableEdit = hasworkflowItem || (!hasworkflowItem && allowedEdit);
    <tr id=""@item.Id"" data-parent-chain='' class= ""doctr  @((item.IsLocalized != null && item.IsLocalized == false) ? ""unlocalized"" : ""localized"") @((item.Published == null || item.Published == false) ? ""unpublished"" : ""published"") @(hasworkflowItem ? ""hasWorkflowItem"" : """")"" >
        <td class=""checkbox mutiple @(ViewBag.Draggable? ""draggable"":"""")"">
            <div>
            @if(ViewBag.Draggable){{
            @Html.IconImage(""drag"")
            }}
            @if (availableEdit)
            {{
                <input type=""checkbox"" name=""select"" class=""select doc"" value=""@item.Id"" data-id-property=""Id"" data-sequence=""@item.Sequence""/>
            }}
            </div>
        </td>
       {1}
        @if (folder.EmbeddedFolders != null)
        {{
            foreach (var s in folder.EmbeddedFolders)
            {{
                var embeddedFolder = Bzway.Data.Models.IPersistableExtensions.AsActual(new TextFolder(Repository.Current, s));
            <td class=""action"">
                @Html.ActionLink(embeddedFolder.FriendlyText, ""SubContent"", new {{ SiteName = ViewContext.RequestContext.GetRequestValue(""SiteName""), RepositoryName = ViewContext.RequestContext.GetRequestValue(""RepositoryName""), ParentFolder = ViewContext.RequestContext.GetRequestValue(""FolderName""), FolderName = s, parentUUID = (object)(item.Id),@return =ViewContext.HttpContext.Request.RawUrl }})
            </td>
            }}
        }}
        @if (Repository.Current.EnableWorkflow && folder.EnabledWorkflow)
        {{
            <td class=""action"">
                @if (hasworkflowItem)
                {{
                    <a href=""@Url.Action(""Process"", ""PendingWorkflow"", ViewContext.RequestContext.AllRouteValues().Merge(""UserKey"", (object)(item.UserKey)).Merge(""Id"", (object)(item.Id)).Merge(""RoleName"", (object)(workflowItem.RoleName)).Merge(""Name"", (object)(workflowItem.Name)).Merge(""return"",Request.RawUrl))"" class=""o-icon process-workflow"">@(""Process workflow"".Localize())</a>
                }}
                else
                {{
                    <a href=""@Url.Action(""WorkflowHistory"", ""PendingWorkflow"", ViewContext.RequestContext.AllRouteValues().Merge(""UserKey"", (object)(item.UserKey)).Merge(""Id"", (object)(item.Id)).Merge(""return"",Request.RawUrl))"" class=""o-icon workflow-history"">@(""Workflow history"".Localize())</a>                      
                }}
            </td>
        }}
        
       @if (schema.IsTreeStyle)
         {{<td class=""action"">
            <a href=""@this.Url.Action(""Create"", ViewContext.RequestContext.AllRouteValues().Merge(""parentFolder"",ViewContext.RequestContext.GetRequestValue(""FolderName"")).Merge(""ParentUUID"", (object)(item.Id)).Merge(""return"",Request.RawUrl))"" >@Html.IconImage(""plus small"")</a>
        </td>}}
    </tr>
        if (Entity.ShowTreeStyle)
        {{
    var nextParentChain  = parentChain + @item.Id + ""="";   
        }}
        }}
}}

<table id=""treeNode-template"" style=""display: none"" data-model=""JsonModel"">
    <tbody data-bind=""foreach:{{data:Entity,as:'item'}}"">
        <tr data-bind='css:item._TRClass_,attr:{{id:item.Id,""data-parent-chain"":item._ParentChain_}}'>
            <td class=""checkbox mutiple @(ViewBag.Draggable? ""draggable"":"""")"">
                <div>
                @if(ViewBag.Draggable){{
                @Html.IconImage(""drag"")
                }}
                <input type=""checkbox"" name=""select"" class=""select doc"" data-bind=""attr:{{id:item.Id, value:item.Id, 'data-sequence':item.Sequence}}"" data-id-property=""Id"" />
                </div>
            </td>
            {4}
            <td class=""date"" data-bind=""html:item._LocalCreationDate_""></td>            
            <td><span data-bind=""text : (item.Published == true?'@(""YES"".Localize())': '-')""></span></td>            
            <!-- ko foreach: {{data:_EmbeddedFolders_,as:'folder'}} -->
            <td >

                <a data-bind=""text:folder.Text,attr:{{href:folder.Link}}"" class=""dialog-link""></a>

            </td>
            <!-- /ko -->
            @if (Repository.Current.EnableWorkflow && folder.EnabledWorkflow)
            {{
                <td>
                    @* @if (hasworkflowItem)
                {{
                    <a href=""@Url.Action(""Process"", ""PendingWorkflow"", ViewContext.RequestContext.AllRouteValues().Merge(""UserKey"", (object)(item.UserKey)).Merge(""Id"", (object)(item.Id)).Merge(""RoleName"", (object)(workflowItem.RoleName)).Merge(""Name"", (object)(workflowItem.Name)).Merge(""return"",Request.RawUrl))"" class=""o-icon process-workflow"">@(""Process workflow"".Localize())</a>
                }}
                else
                {{
                    <a href=""@Url.Action(""WorkflowHistory"", ""PendingWorkflow"", ViewContext.RequestContext.AllRouteValues().Merge(""UserKey"", (object)(item.UserKey)).Merge(""Id"", (object)(item.Id)).Merge(""return"",Request.RawUrl))"" class=""o-icon workflow-history"">@(""Workflow history"".Localize())</a>
                }}*@
                </td>
            }}           
            @if (schema.IsTreeStyle)
            {{
                <td class=""action"">
                    <a data-bind=""attr:{{href:item._CreateUrl_}}"">@Html.IconImage(""plus small"")</a>
                </td>
            }}
        </tr>
    </tbody>
</table>
";

        #region ISchemaTemplate Members

        public string Generate(ISchema schema)
        {
            StringBuilder sb_head = new StringBuilder();

            StringBuilder sb_body = new StringBuilder();

            StringBuilder columnDataSource = new StringBuilder();
            StringBuilder sb_koTml = new StringBuilder();

            int colspan = 0;
            foreach (var item in schema.Columns.OrderBy(it => it.Order))
            {
                if (item.ShowInGrid)
                {
                    string columnValue = string.Format("@Bzway.Web.Form.Html.HtmlCodeHelper.RenderColumnValue(item.{0})", item.Name);
                    if (HasDataSource(item.ControlType))
                    {
                        if (!string.IsNullOrEmpty(item.SelectionFolder))
                        {
                            //                        sb_categoryData.AppendFormat(@"
                            //                          
                            //                         ", item.Name, item.SelectionFolder);
                            columnDataSource.AppendFormat(@"var {0}_data = (new TextFolder(Repository.Current,""{1}"")).CreateQuery().ToArray();", item.Name, item.SelectionFolder);
                            columnValue = string.Format(@"@{{
                        string {0}_rawValue = (item.{0} ?? """").ToString();
                        string[] {0}_value = {0}_rawValue.Split(new[] {{ ',' }}, StringSplitOptions.RemoveEmptyEntries);                        
                        var {0}_values = {0}_data.Where(it =>
                            {0}_value.Any(s =>
                                s.EqualsOrNullEmpty(it.Id, StringComparison.OrdinalIgnoreCase))).ToArray();}}
                        @if ({0}_values.Length > 0)
                        {{
                            @(string.Join("","", {0}_values.Select(it => it.GetSummary())))
                        }}
                        else
                        {{
                            {1}
                        }}", item.Name, columnValue, item.SelectionFolder);
                        }
                        else if (item.SelectionItems != null && item.SelectionItems.Length > 0)
                        {
                            columnDataSource.AppendFormat(@"var {0}_data = schema[""{0}""].SelectionItems;", item.Name);
                            columnValue = string.Format(@"@{{
                        string {0}_rawValue = (item.{0} ?? """").ToString();
                        string[] {0}_value = {0}_rawValue.Split(new[] {{ ',' }}, StringSplitOptions.RemoveEmptyEntries);
                      
                        var {0}_values = {0}_data.Where(it =>
                            {0}_value.Any(s =>
                                s.EqualsOrNullEmpty(it.Value, StringComparison.OrdinalIgnoreCase))).ToArray();}}
                        @if ({0}_values.Length > 0)
                        {{
                            @(string.Join("","", {0}_values.Select(it => it.Text)))
                        }}
                        else
                        {{
                            {1}
                        }}", item.Name, columnValue, item.SelectionFolder);
                        }
                    }
                    sb_head.AppendFormat("\t\t<th class=\"{1} @SortByExtension.RenderSortHeaderClass(ViewContext.RequestContext, \"{1}\",-1)\">@SortByExtension.RenderGridHeader(ViewContext.RequestContext, \"{0}\", \"{1}\", -1)</th>\r\n", string.IsNullOrEmpty(item.Label) ? item.Name : item.Label, item.Name, colspan);
                    if (item.Name.EqualsOrNullEmpty("Published", StringComparison.CurrentCultureIgnoreCase))
                    {
                        sb_body.AppendFormat("\t\t<td>{0}</td>", columnValue);
                    }
                    else if (item.Name.EqualsOrNullEmpty("UtcCreationDate", StringComparison.CurrentCultureIgnoreCase))
                    {
                        sb_body.AppendFormat("\t\t<td class=\"date\">@(DateTime.Parse(item[\"{0}\"].ToString()).ToLocalTime().ToShortDateString())</td>\r\n", item.Name);
                    }
                    else if (item.DataType == ColumnType.DateTime)
                    {
                        sb_body.AppendFormat("\t\t<td class=\"date\">@(item[\"{0}\"] == null?\"\":((DateTime)item[\"{0}\"]).ToLocalTime().ToShortDateString())</td>\r\n", item.Name);
                    }
                    else
                    {
                        if (colspan == 0)
                        {
                            sb_body.AppendFormat("\t\t<td>@if(Entity.ShowTreeStyle){{\n\t\t<span class=\"expander\">@Html.IconImage(\"arrow\")</span>}}\n<a href=\"@this.Url.Action(\"Edit\",\"TextContent\",ViewContext.RequestContext.AllRouteValues().Merge(\"UserKey\", (object)(item.UserKey)).Merge(\"Id\",(object)(item.Id)).Merge(\"return\",Request.RawUrl))\">@Html.IconImage(\"file document\"){1}</a></td>\r\n"
                                , schema.Name, columnValue);

                            sb_koTml.AppendFormat("\t\t<td class=\"treeStyle\">\n\t\t<span class=\"expander\">@Html.IconImage(\"arrow\")</span>\n\t\t<a data-bind=\"attr:{{href:item._EditUrl_}}\">@Html.IconImage(\"file document\")<!--ko text: item.{0}--><!--/ko--></a></td>"
                                , item.Name);
                        }
                        else
                        {
                            sb_body.AppendFormat("\t\t<td>{0}</td>\r\n", columnValue);
                            sb_koTml.AppendFormat("\t\t<td data-bind=\"html:item.{0}\"></td>", item.Name);
                        }

                    }

                    colspan++;
                }
            }

            return string.Format(Table, sb_head, sb_body, colspan - 1, columnDataSource, sb_koTml);
        }
        private bool HasDataSource(string controlType)
        {
            if (string.IsNullOrEmpty(controlType))
            {
                return false;
            }
            var control = ControlHelper.Resolve(controlType);
            if (control == null)
            {
                return false;
            }
            return control.HasDataSource;
        }
        #endregion
    }
}