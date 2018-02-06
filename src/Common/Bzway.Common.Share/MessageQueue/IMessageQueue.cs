using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Bzway.Common.Share
{
    public interface IMessageQueue<T>
    {
        void Subscribe(string queue, Action<T> action, int millisecondsTimeout = 1000);
        void Publish(T data, string queue = "");
    }
}