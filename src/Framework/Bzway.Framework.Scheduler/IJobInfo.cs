using Quartz;
using System.Collections.Generic;

namespace Bzway.Framework.Scheduler
{
    public interface IJobInfo
    {
        string[] Settings { get; }
        object Invoke();
    }
}