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

namespace OpenData.Framework.Common.Css
{
    public class AtRule : Statement
    {
        public const char StartToken = '@';
        public const char EndToken = ';';

        public static AtRule Parse(string str)
        {
            return new AtRule();
        }
    }
}
