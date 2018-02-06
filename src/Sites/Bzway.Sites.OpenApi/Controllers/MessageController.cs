using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bzway.Framework.Application;
using Bzway.Database.Core;
using Bzway.Sites.OpenApi.Models;
using Bzway.Common.Share;

namespace Bzway.Sites.OpenApi.Controllers
{
    [Route("message")]
    public class MessageController : Controller
    {
        readonly ISystemDatabase db;
        readonly ICacheManager cache;
        readonly IMessageQueue<MessageResponseModel> messageQueue;
        public MessageController()
        {

            this.cache = AppEngine.GetService<ICacheManager>("Redis");
            this.messageQueue = MessageQueueManager.Default.GetMessage<MessageResponseModel>();
        }

        // GET api/values/5
        [HttpGet("{type}")]
        public Result<List<MessageResponseModel>> Get(string a, string b, string c, string type)
        {
            //confirm message
            if (!string.IsNullOrEmpty(c))
            {
                foreach (var item in c.Split(';', ','))
                {
                    //todo;
                }
            }
            if (!string.IsNullOrEmpty(a))
            {
                //todo:getdata from database where the message after :a
                return Result<List<MessageResponseModel>>.Success(null);
            }
            if (!string.IsNullOrEmpty(b))
            {
                //todo:getdata from database where the message before :b
                return Result<List<MessageResponseModel>>.Success(null);
            }
            //todo: getdate from redis cache;
            List<MessageResponseModel> list = new List<MessageResponseModel>();
            this.messageQueue.Subscribe("messages:list", m =>
            {
                list.Add(m);
            }, 1000000);
            return Result<List<MessageResponseModel>>.Success(list,list.Count().ToString());
        }


        /// <summary>
        /// start session
        /// </summary>
        /// <param name="to"></param>
        [HttpPost]
        public string Start(string[] to)
        {
            if (to.Length == 0)
            {
                return "@" + this.User.ToString();
            }
            if (to.Length == 1)
            {
                return "@" + to;
            }
            var groupId = Guid.NewGuid().ToString("N");
            foreach (var item in to.OrderBy(m => m))
            {

            }
            return "@@" + groupId;
        }
        [HttpPost("Send")]
        public void Send(string SessionId, string Type, string Content)
        {
            for (int i = 0; i < 100; i++)
            {
                this.messageQueue.Publish(new MessageResponseModel() { Type = "test" + i, Data = new { NickName = "Mike" + i, Age = 123 } }, "messages:list");

            }
        }

        [HttpPost]
        public void Push(string SessionId, string Type, string Content)
        {
        }


    }
}