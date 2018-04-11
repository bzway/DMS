using Bzway.Database.Core;
using System;

namespace Bzway.Framework.Application.Entity
{
    public class LanguageResource : EntityBase
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Language { get; set; }
    } 
}