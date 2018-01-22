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
using System.Web;

namespace OpenData.Framework.Common
{
    public static class StringEx
    {
        public static string HtmlAttributeEncode(this string source)
        {
            return HttpUtility.HtmlAttributeEncode(source);
        }
    }
}
