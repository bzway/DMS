using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Bzway.Common.Share
{
    /// <summary>
    /// 
    /// </summary>
    public class BzwayException : Exception
    {
        public BzwayException(string message) : base(message) { }
    }
}