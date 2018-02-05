using Newtonsoft.Json;
using System;
using System.Collections.Generic;
namespace Bzway.Sites.OpenApi.Models
{
    public class UploadFileRequestModel
    {
        public string Type { get; set; }
        public string Path { get; set; }
        public string Title { get; set; }
        public string Remark { get; set; }
    }
    public class UploadFileResponseModel
    {
        public string Type { get; set; }
        public string FileId { get; set; }
        public long CreateTime { get; set; }
    }
}
