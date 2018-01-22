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

namespace OpenData.Framework.Common.Css.Meta
{
    public class FontFamilyType : PropertyValueType
    {
        public static readonly EnumType GenericFamily = new EnumType("serif | sans-serif | cursive | fantasy | monospace");

        public override string DefaultValue
        {
            get { return String.Empty; }
        }

        public override bool IsValid(string value)
        {
            return IsFamilyName(value) || IsGenericFamilyName(value);
        }

        public override string Standardlize(string value)
        {
            return value.Replace("\"", "'");
        }

        private bool IsFamilyName(string value)
        {
            return true;
        }

        private bool IsGenericFamilyName(string value)
        {
            return GenericFamily.IsValid(value);
        }
    }
}
