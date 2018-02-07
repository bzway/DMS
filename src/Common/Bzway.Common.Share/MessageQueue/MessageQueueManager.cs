using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Bzway.Common.Share
{

    public class MessageQueueManager
    {
        static readonly Lazy<MessageQueueManager> lazy = new Lazy<MessageQueueManager>(() => { return new MessageQueueManager(); });

        private MessageQueueManager()
        {

        }
        public static MessageQueueManager Default
        {
            get
            {
                return lazy.Value;
            }
        }
        public IMessageQueue<T> CreateMessageQueue<T>(string name = "Redis")
        {
            if ("Redis".Equals(name))
            {
                return new RedisMessageQueue<T>();
            }
            else
            {
                return new RabbitMessageQueue<T>();
            }
        }
    }
}