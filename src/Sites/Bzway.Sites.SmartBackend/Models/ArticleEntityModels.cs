using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Bzway.Database.Core;

namespace Bzway.Sites.SmartBackend.Models
{
    public class Article : EntityBase
    {
        public string Categroy { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string Remark { get; set; }
        public string ProductTags { get; set; }
    }
}
