using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bzway.Framework.Application;
using Bzway.Database.Core;
using Bzway.Sites.OpenApi.Models;

namespace Bzway.Sites.OpenApi.Controllers
{
    [Route("api/[controller]")]
    public class MessageController : Controller
    {
        readonly ISystemDatabase db;
        public MessageController(ISystemDatabase db)
        {
            this.db = db;
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
            return Result<List<MessageResponseModel>>.Success(null);
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
        [HttpPost]
        public void Send(string SessionId, string Type, string Content)
        {

        }

        [HttpPost]
        public void Push(string SessionId, string Type, string Content)
        {
        }


    }
}