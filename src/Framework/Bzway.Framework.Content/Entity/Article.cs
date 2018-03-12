using Bzway.Data.Core;
using System;

namespace Bzway.Framework.Content.Entity
{
    public class Article : EntityBase
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string ThumbnailId { get; set; }
    }
}