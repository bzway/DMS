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

namespace Bzway.Mvc
{
    public enum FrontRequestChannel
    {
        Unknown,
        /// <summary>
        /// s~site1
        /// </summary>
        Debug,
        /// <summary>
        /// www.site1.com
        /// </summary>
        Host,
        /// <summary>
        /// www.Bzway.com/site1
        /// </summary>
        HostNPath,
        /// <summary>
        /// 
        /// </summary>
        Design,
        Draft
    }
}
