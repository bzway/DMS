using Quartz;
using System;

namespace Bzway.Framework.Scheduler
{
    public class JobDescription
    {
        public string Group { get; internal set; }
        public string Name { get; internal set; }
        public string Description { get; internal set; }
        public string TriggerName { get; internal set; }
        public string TriggerType { get; internal set; }
        public TriggerState TriggerState { get; internal set; }
        public DateTimeOffset? NextFireTime { get; internal set; }
        public DateTimeOffset? PreviousFireTime { get; internal set; }
    }
}