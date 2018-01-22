using System;
using System.Security;
using System.Linq;
using System.Net;

namespace Bzway.Common.Utility
{
    public struct ObjectId
    {
        #region Ctor
        private readonly byte[] value;

        ObjectId(long value)
        {
            this.value = BitConverter.GetBytes(value);
        }

        ObjectId(byte[] bytes)
        {
            this.value = bytes;
        }
        ObjectId(string input)
        {
            var chars = input.ToArray();
            this.value = new byte[12];
            for (var i = 0; i < value.Length; i += 2)
            {
                this.value[i / 2] = Convert.ToByte(new string(chars, i, 2), 16);
            }
        }
        public DateTime DateTime
        {
            get
            {
                if (this.value.Length == 8)
                {
                    return TimeStamp.GetDateTime(BitConverter.ToInt64(this.value, 0));
                }
                else
                {
                    var input = new byte[4];
                    for (int i = 0; i < input.Length; i++)
                    {
                        input[i] = this.value[3 - i];
                    }
                    return TimeStamp.GetDateTime(BitConverter.ToInt32(input, 0));
                }
            }
        }
        public override string ToString()
        {
            if (this.value.Length == 8)
            {
                return BitConverter.ToInt64(this.value, 0).ToString();
            }
            else
            {
                return BitConverter.ToString(this.value);
            }
        }
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        #endregion

        [SecuritySafeCritical]
        public static ObjectId New()
        {
            var timeStamp = TimeStamp.GetTimeStamp();
            var machineId = Machine.GetMachineId();
            var count = Counter.GetNumber();
            return new ObjectId(timeStamp | machineId | count);
        }
        public static ObjectId Parse(string input)
        {
            long value;
            if (long.TryParse(input, out value))
            {
                return new ObjectId(value);
            }
            else
            {
                return new ObjectId(input);
            }
        }

        internal class TimeStamp
        {
            private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            private static readonly DateTime BaseTime = new DateTime(2017, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            public static readonly int lenthOfTimeStamp = 42;//机器码长度
            public static readonly long maxTimeStamp = 1L << lenthOfTimeStamp; //最大机器码
            public static long GetTimeStamp()
            {
                var timeSpan = DateTime.UtcNow - BaseTime;
                var milliSeconds = (long)timeSpan.TotalMilliseconds;
                var result = milliSeconds << (64 - lenthOfTimeStamp);
                return result;
            }
            public static DateTime GetDateTime(long input)
            {
                double value = input >> (64 - lenthOfTimeStamp);
                return BaseTime.AddMilliseconds(value);
            }
            public static DateTime GetEpochDateTime(double input)
            {
                return Epoch.AddSeconds(input);
            }
        }
        internal class Machine
        {
            private static long machineId = 0;
            public static readonly int lenthOfMachineId = 8;//机器码长度
            public static readonly int maxMachineId = 1 << lenthOfMachineId; //最大机器码

            public static long GetMachineId()
            {
                if (machineId <= 0)
                {
                    machineId = (long)Math.Abs(Dns.GetHostName().GetHashCode()) % maxMachineId;
                }
                return machineId << 14;
            }
        }
        internal class Counter
        {
            private static ushort count;
            public static readonly int lenthOfCount = 14;//计数长度
            public static readonly int maxCount = 1 << lenthOfCount; //最大计数
            private static object syncRoot = new object();
            public static ushort GetNumber()
            {
                if (count > maxCount)
                {
                    count = 0;
                }
                lock (syncRoot)
                {
                    return count++;
                }
            }
        }
    }
}