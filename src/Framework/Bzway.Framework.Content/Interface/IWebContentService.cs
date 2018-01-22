
using Bzway.Framework.Content.Entity;
using System.Collections.Generic;

namespace Bzway.Framework.Content
{
    public interface IWebContentService
    {
        WebPage FindPage(string PageUrl);
    }
}
