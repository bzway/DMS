using Bzway.Database.Core;
using System;

namespace Bzway.Framework.Content.Entity
{
    public class MediaFile
    {
        public string Name { get; set; }
        public string MediaType { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
    }
}