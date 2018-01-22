using Bzway.Database.Core;
using System;

namespace Bzway.Framework.Application.Entity
{
    public class Client : EntityBase
    {
        public string Name { get; set; }
        public string Descrtption { get; set; }
        public string AppKey { get; set; }
        public string AppSecret { get; set; }
    }
}