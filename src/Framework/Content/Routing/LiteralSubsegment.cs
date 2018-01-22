﻿#region License
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

namespace System.Web.Routing
{
    public sealed class LiteralSubsegment : PathSubsegment
    {
        // Methods
        public LiteralSubsegment(string literal)
        {
            this.Literal = literal;
        }

        // Properties
        public string Literal { get; private set; }
    }


}
