using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Bzway.Common.Share
{
    public class MessageQueueManager
    {
        #region ctor
        static object lockObject = new object();
        static MessageQueueManager manager;
        readonly ConnectionMultiplexer connection;
        MessageQueueManager(ConnectionMultiplexer connection)
        {
            this.connection = connection;
        }
        #endregion
        public static MessageQueueManager Default
        {
            get
            {
                if (manager == null)
                {
                    lock (lockObject)
                    {
                        if (manager == null)
                        {
                            manager = new MessageQueueManager(ConnectionMultiplexer.Connect("localhost"));
                        }
                    }
                }
                return manager;
            }
        }

        public IMessageQueue<T> GetMessage<T>(string name = "Redis")
        {
            if ("Redis".Equals(name))
            {
                return new RedisMessageQueue<T>(this.connection);
            }
            else
            {
                return new RabbitMessageQueue<T>();
            }
        }
    }
}