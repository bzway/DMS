using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bzway.Common.Share
{


    public class RabbitMessageQueue<T> : IMessageQueue<T>
    {
        static ConnectionFactory factory = new ConnectionFactory { HostName = "localhost" };
        public void Publish(T data, string queue = "")
        {
            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue, false, false, false, null);
                    byte[] payload = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
                    channel.BasicPublish("", queue, null, payload);
                }
            }
        }
        public void Subscribe(string queue, Action<T> action, int millisecondsTimeout = 1000)
        {
            IConnection connection = factory.CreateConnection();
            {
                IModel channel = connection.CreateModel();
                {
                    channel.QueueDeclare(queue, false, false, false, null);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, e) =>
                    {
                        consumer.Model.Close();
                        connection.Close();
                        var body = e.Body;     //消息主体
                        var message = Encoding.UTF8.GetString(body);
                        action(JsonConvert.DeserializeObject<T>(message));
                    };
                    var result = channel.BasicConsume(queue, true, consumer);
                }
            }
        }
    }


    public class RabbitMessageQueue : IMessageQueue<string>
    {
        static ConnectionFactory factory = new ConnectionFactory { HostName = "localhost" };
        public void Publish(string data, string queue = "")
        {
            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue, false, false, false, null);
                    byte[] payload = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
                    channel.BasicPublish("", queue, null, payload);
                }
            }
        }
        public void Subscribe(string queue, Action<string> action,int millisecondsTimeout = 1000)
        {

            IConnection connection = factory.CreateConnection();
            {
                IModel channel = connection.CreateModel();
                {
                    channel.QueueDeclare(queue, false, false, false, null);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, e) =>
                    {
                        consumer.Model.Close();
                        connection.Close();
                        var body = e.Body;     //消息主体
                        var message = Encoding.UTF8.GetString(body);
                        action(JsonConvert.DeserializeObject<string>(message));
                    };
                    var result = channel.BasicConsume(queue, true, consumer);

                }
            }
        }
    }
}