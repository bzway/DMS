using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bzway.Framework.Application;

namespace Bzway.Sites.OpenApi.Controllers
{
    public class Model
    {
        public string media_id { get; set; }
    }
    [Route("api/[controller]")]
    public class MaterialController : Controller
    {
        // GET api/values
        [HttpPost]
        public IEnumerable<string> GetMaterial(string access_token, Model model)
        {
            return new string[] { model.media_id, access_token };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            var tenant = (ITenant)this.HttpContext.RequestServices.GetService(typeof(ITenant));

            return tenant.GetContext().Request.Path;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
