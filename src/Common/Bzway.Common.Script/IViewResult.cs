using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bzway.Common.Script
{
    public interface IViewResult
    {
        Task<string> Render();
        IDictionary<string, object> MapData { get; }
    }
}