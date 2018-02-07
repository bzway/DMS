using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bzway.Common.Share
{
    public class RedisMessageQueue : IMessageQueue<string>
    {
        readonly ConnectionMultiplexer connection;
        public RedisMessageQueue(ConnectionMultiplexer connection)
        {
            this.connection = connection;
        }
        public void Publish(string data, string channel)
        {
            var subscriber = connection.GetSubscriber();
            subscriber.Publish(channel, data);
        }

        public void Subscribe(string channel, Action<string> action, int millisecondsTimeout = 1000)
        {
            var subscriber = connection.GetSubscriber();
            subscriber.Subscribe(channel, (c, value) => { action(value); });
        }
    }


    public class RedisMessageQueue<T> : IMessageQueue<T>
    {
        readonly ConnectionMultiplexer connection;
        public RedisMessageQueue()
        {
            this.connection = ConnectionMultiplexer.Connect("localhost");
        }
        public virtual void Publish(T data, string channel)
        {
            var subscriber = connection.GetSubscriber();
            var db = this.connection.GetDatabase();
            var key = channel + ":data";
            db.ListRightPush(key, JsonConvert.SerializeObject(data));
            subscriber.Publish(channel, JsonConvert.SerializeObject(data));
        }

        public virtual void Subscribe(string channel, Action<T> action, int millisecondsTimeout = 1000)
        {
            bool hasValue = false;
            var db = this.connection.GetDatabase();
            var key = channel + ":data";
            RedisValue value;
            do
            {
                value = db.ListRightPop(key);
                if (value.HasValue)
                {
                    hasValue = true;
                    var receivced = JsonConvert.DeserializeObject<T>(value);
                    action(receivced);
                }
            } while (value.HasValue);

            if (hasValue)
            {
                return;
            }
            var watch = Stopwatch.StartNew();
            watch.Start();

            using (ManualResetEvent blocker = new ManualResetEvent(false))
            {
                var subscriber = this.connection.GetSubscriber();
                subscriber.Subscribe(channel, (c, v) =>
                {
                    blocker.Set();
                });
                while (true)
                {
                    millisecondsTimeout -= (int)watch.ElapsedMilliseconds;
                    if (millisecondsTimeout <= 0)
                    {
                        return;
                    }
                    blocker.WaitOne(millisecondsTimeout);
                    do
                    {
                        value = db.ListRightPop(key);
                        if (value.HasValue)
                        {
                            hasValue = true;
                            var receivced = JsonConvert.DeserializeObject<T>(value);
                            action(receivced);
                        }
                    } while (value.HasValue);
                    if (hasValue)
                    {
                        return;
                    }
                    blocker.Reset();
                }
            }

        }
    }
}