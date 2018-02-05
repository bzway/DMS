using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;
using Bzway.Framework.Application;
using Microsoft.Extensions.Logging;
using Bzway.Sites.OpenApi.Models;

namespace Bzway.Sites.OpenApi.Controllers
{
    [Route("File")]
    public class FileController : BaseController<FileController>
    {
        public FileController(ILoggerFactory loggerFactory) : base(loggerFactory)
        {

        }
        [HttpPost]
        [Route("Upload")]
        public Result<UploadFileResponseModel> Upload(UploadFileRequestModel model)
        {
            var file = this.Request.Form.Files["File"];
            if (file == null)
            {
                Result<UploadFileResponseModel>.Fail(ResultCode.BadRequest, "No file upload");
            }
            var root = $"d:\\files\\{this.User.Identity.Name}\\";
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            string fileId = Guid.NewGuid().ToString("N");
            var path = Path.Combine(root, fileId);
            using (FileStream fileStream = System.IO.File.Create(path))
            {
                file.OpenReadStream().CopyTo(fileStream);
            }

            return Result<UploadFileResponseModel>.Success(new UploadFileResponseModel()
            {
                CreateTime = DateTime.UtcNow.Ticks,
                FileId = fileId,
                Type = string.Empty,
            });
        }

        // PUT api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(string id, string type)
        {
            var root = $"d:\\files\\{this.User.Identity.Name}\\";
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            var path = Path.Combine(root, $"{id}.{type}");
            if (System.IO.File.Exists(path))
            {
                return File(System.IO.File.OpenRead(path), contentType: string.Empty, fileDownloadName: string.Empty);
            }
            return NotFound();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
