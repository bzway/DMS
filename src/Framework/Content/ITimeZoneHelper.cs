#region License
// 
// Copyright (c) 2013, Bzway team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using OpenData.AppEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Web
{
    public interface ITimeZoneHelper
    {
        TimeZoneInfo GetCurrentTimeZone();

        TimeZoneInfo FindTimeZoneById(string id);

        IEnumerable<TimeZoneInfo> GetTimeZones();

        DateTime ConvertToUtcTime(DateTime dt);

        DateTime ConvertToLocalTime(DateTime dt, TimeZoneInfo sourceTimeZone);

    }

    public static class TimeZoneHelper
    {
        public static TimeZoneInfo GetCurrentTimeZone()
        {
            return ApplicationEngine.Current.Resolve<ITimeZoneHelper>().GetCurrentTimeZone();
        }

        public static TimeZoneInfo FindTimeZoneById(string id)
        {
            return ApplicationEngine.Current.Resolve<ITimeZoneHelper>().FindTimeZoneById(id);
        }

        public static IEnumerable<TimeZoneInfo> GetTimeZones()
        {
            return ApplicationEngine.Current.Resolve<ITimeZoneHelper>().GetTimeZones();
        }

        public static DateTime ConvertToUtcTime(DateTime dt)
        {
            return ApplicationEngine.Current.Resolve<ITimeZoneHelper>().ConvertToUtcTime(dt);
        }

        public static DateTime ConvertToLocalTime(DateTime dt, TimeZoneInfo sourceTimeZone)
        {
            return ApplicationEngine.Current.Resolve<ITimeZoneHelper>().ConvertToLocalTime(dt, sourceTimeZone);
        }
    }
}