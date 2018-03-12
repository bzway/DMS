using System;
namespace Bzway.Common.Utility
{

    public static class DateTimeHelper
    {
        static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static DateTime GetEpochDateTime(long input)
        {
            return epoch.AddSeconds(input);
        }
        public static DateTime GetEpochDateTime(double input)
        {
            return epoch.AddSeconds(input);
        }
        public static double GetBaseTimeValue(this DateTime input)
        {
            input = input.ToUniversalTime();
            return (input - epoch).TotalSeconds;
        }
    }
}