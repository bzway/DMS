using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;
using Bzway.Framework.Application;
using Microsoft.Extensions.Logging;
using Bzway.Sites.BackOffice.Models;
using Bzway.Framework.DistributedFileSystemClient;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Net.Http.Headers;
using Bzway.Framework.DistributedFileSystemClient;

namespace Bzway.Sites.BackOffice.Controllers
{
    [Route("[controller]/[action]")]
    public class FileController : BaseController<FileController>
    {
        #region ctor
        private readonly FileMetaData fileBrowser;
        private readonly IDistributedFileSystemService staticFileService;
        public FileController(FileMetaData fileBrowser, IDistributedFileSystemService staticFileService, ITenant tenant, ILoggerFactory loggerFactory) : base(loggerFactory, tenant)
        {
            this.staticFileService = staticFileService;
            this.fileBrowser = fileBrowser;
        }
        #endregion


        public IActionResult Index()
        {
            return View();
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


        [HttpGet]
        public IActionResult List(string path)
        {
            var list = this.fileBrowser.Get(path);
            if (list.Count == 1 && list[0].IsFile)
            {
                return File(list[0].Path, "image/jpg");
            }
            return Json(list);
        }
        [HttpPost]
        public IActionResult Post(string path)
        {
            if (this.Request.Form.Files != null && this.Request.Form.Files.Count > 0)
            {
                var files = this.Request.Form.Files.Select(m => new WebFilePost()
                {
                    ContentDisposition = m.ContentDisposition,
                    ContentType = m.ContentType,
                    FileName = m.FileName,
                    //Headers = m.Headers,
                    Length = m.Length,
                    Name = m.Name,
                    OpenReadStream = m.OpenReadStream(),
                }).Cast<IWebFilePost>().ToList();
                this.fileBrowser.CreateFile(path, files, true);
                return Json("OK");
            }
            this.fileBrowser.Create(path);
            return Json("OK");
        }
        [HttpDelete]
        public IActionResult Delete(string path)
        {
            this.fileBrowser.Delete(path, true);
            return Json("OK");
        }
    }
}
