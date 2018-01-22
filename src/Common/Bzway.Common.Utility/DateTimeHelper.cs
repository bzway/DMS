using System;
namespace Bzway.Common.Utility
{
    /*
    DateTime baseTime = new DateTime(1970, 1, 1);
    var expected = (long)(DateTime.UtcNow - baseTime).TotalSeconds;
    var v1 = DateTime.Now.ToFileTimeUtc();
    var v2 = DateTime.UtcNow.ToFileTime();
    Assert.Equal(expected, v1);
    Assert.Equal(expected, v2);
    */
    public class DateTimeHelper
    {
        static readonly DateTime baseTime = new DateTime(1970, 1, 1);
        public static DateTime ConvertToBaseTime(long input)
        {
            return baseTime.AddSeconds(input);
        }
        public static long GetBaseTimeValue(DateTime input)
        {
            input = input.ToUniversalTime();
            return (long)(input - baseTime).TotalSeconds;
        }
    }
}