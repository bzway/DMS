using Bzway.Database.Core;
using System;

namespace Bzway.Framework.Content.Entity
{
    public class WebPage : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string AppKey { get; set; }
        public string AppSecret { get; set; }
    }
}