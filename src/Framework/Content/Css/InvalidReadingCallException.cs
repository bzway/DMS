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
    [Serializable]
    public class InvalidReadingCallException : Exception
    {
        public InvalidReadingCallException() { }
        public InvalidReadingCallException(string message) : base(message) { }
        public InvalidReadingCallException(string message, Exception inner) : base(message, inner) { }
        protected InvalidReadingCallException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
