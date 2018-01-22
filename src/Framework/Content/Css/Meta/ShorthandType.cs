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
    public class ShorthandType : PropertyValueType
    {
        private PropertyValueType _defaultValueType;

        public ShorthandType(ShorthandRule rule)
            : this(rule, null)
        {
        }

        public ShorthandType(ShorthandRule rule, PropertyValueType defualtValueType)
        {
            _defaultValueType = defualtValueType;
            ShorthandRule = rule;
        }

        public ShorthandRule ShorthandRule { get; private set; }

        public override string DefaultValue
        {
            get { return _defaultValueType.DefaultValue; }
        }

        public override bool IsValid(string value)
        {
            return _defaultValueType.IsValid(value);
        }
    }
}
