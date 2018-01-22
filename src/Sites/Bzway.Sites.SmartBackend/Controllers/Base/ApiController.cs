using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bzway.Framework.Application;
using Bzway.Database.Core;
using Microsoft.Extensions.Logging;

namespace Bzway.Sites.SmartBackend.Controllers
{
    public class ApiController<E, T> : BaseController<T> where E : EntityBase, new()
    {
        protected readonly IRepository<E> db;
        protected readonly ISystemDatabase repository;
        public ApiController(ILoggerFactory loggerFactory, ITenant tenant) : base(loggerFactory, tenant)
        {
            this.repository = SystemDatabase.GetDatabase(tenant.Site.ProviderName, tenant.Site.ConnectionString, tenant.Site.DatabaseName);
            this.db = this.repository.Entity<E>();
        }
        [HttpGet]
        public IEnumerable<E> List()
        {
            return this.db.Query().ToList();
        }

        [HttpGet]
        public E Item(string id)
        {
           return this.db.Query().Where(m => m.Id == id).FirstOrDefault();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]E value)
        {
            this.db.Insert(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody]E value)
        {
            var item = this.db.Query().Where(m => m.Id == id).First();
            if (item != null)
            {
                this.db.Update(value, id);
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            this.db.Delete(id);
        }
    }
}
