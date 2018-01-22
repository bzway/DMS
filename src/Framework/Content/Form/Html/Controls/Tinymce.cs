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
	public class Tinymce : ControlBase
	{
		public override string Name
		{
			get { return "Tinymce"; }
		}

		protected override string RenderInput(IColumn column)
		{
			return string.Format(@"
<textarea name=""{0}"" id=""{0}"" class=""{0} tinymce"" media_library_url=""@Url.Action(""Selection"",""MediaContent"",ViewContext.RequestContext.AllRouteValues()))""  media_library_title =""@(""Selected Files"".Localize())"" rows=""10"" cols=""100"">@( Entity.{0} ?? """")</textarea>
", column.Name);
		}
	}
}
