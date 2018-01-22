#region License
// 
// Copyright (c) 2013, Bzway team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;

namespace OpenData.HealthMonitoring
{
    /// <summary>
    /// 
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Log" /> class.
        /// </summary>
        /// <param name="e">The e.</param>
        public static Action<Exception> Logger = (Exception e) =>
        {
            var webEvent = new WebRequestErrorEventWrapper(e.Message, null, 100000, e);
            webEvent.Raise();
        };
        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="e">The e.</param>
        public static void LogException(Exception e)
        {
            Logger(e);
        }
    }
}