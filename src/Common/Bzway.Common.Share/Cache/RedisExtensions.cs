using StackExchange.Redis;
using System.Diagnostics;
using System.Threading;

namespace Bzway.Common.Share
{
    public static class RedisExtensions
    {
        static readonly string FormatKeyString = "Queue.List.Key.{0}";

        public static RedisValue SubscribeFromQueue(this ISubscriber subscriber, RedisChannel channel, int millisecondsTimeout = 1000)
        {

            var watch = Stopwatch.StartNew();
            watch.Start();
            //try to get value from db - Queue
            var key = string.Format(FormatKeyString, channel);
            var db = subscriber.Multiplexer.GetDatabase();
            var value = db.ListRightPop(key);
            if (value.HasValue)
            {
                return value;
            }
            //if without data start a subscriber
            using (ManualResetEvent blocker = new ManualResetEvent(false))
            {
                subscriber.Subscribe(channel, (c, v) =>
                {
                    blocker.Set();
                });

                while (true)
                {
                    millisecondsTimeout -= (int)watch.ElapsedMilliseconds;
                    if (!blocker.WaitOne(millisecondsTimeout))
                    {
                        return RedisValue.Null;
                    }
                    value = db.ListRightPop(key);
                    if (value.HasValue)
                    {
                        return value;
                    }
                    blocker.Reset();
                }
            }
        }

        public static long PublishToQueue(this ISubscriber subscriber, RedisChannel channel, RedisValue message, CommandFlags flags = CommandFlags.None)
        {
            var key = string.Format(FormatKeyString, channel);
            //get database store message to Queue
            var db = subscriber.Multiplexer.GetDatabase();
            db.ListLeftPush(key, message);
            return subscriber.Publish(channel, RedisValue.EmptyString);
        }
    }
}