using System.Collections.Generic;

namespace Bzway.Common.Script
{
    public interface IUrlMapBuilder
    {
        UrlMaper Build();
        UrlMapBuilder Map(string template, params KeyValuePair<string, string>[] defaults);
    }
}