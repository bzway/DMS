using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

        public void Subscribe(string channel, Action<string> action)
        {
            var subscriber = connection.GetSubscriber();
            subscriber.Subscribe(channel, (c, value) => { action(value); });
        }
    }


    public class RedisMessageQueue<T> : IMessageQueue<T>
    {
        readonly ConnectionMultiplexer connection;
        public RedisMessageQueue(ConnectionMultiplexer connection)
        {
            this.connection = connection;
        }
        public virtual void Publish(T data, string channel)
        {
            var subscriber = connection.GetSubscriber();
            subscriber.Publish(channel, JsonConvert.SerializeObject(data));
        }

        public virtual void Subscribe(string channel, Action<T> action)
        {
            var subscriber = connection.GetSubscriber();
            subscriber.Subscribe(channel, (c, v) =>
            {
                var receivced = JsonConvert.DeserializeObject<T>(v);
                action(receivced);
            });
        }
    }
}