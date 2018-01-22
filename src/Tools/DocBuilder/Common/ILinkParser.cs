using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Bzway.Common.Share
{
    public interface ILinkParser
    {
        List<string> Links { get; }
    }
}