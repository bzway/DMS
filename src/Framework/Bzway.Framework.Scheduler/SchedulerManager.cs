using Bzway.Common.Utility;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Bzway.Framework.Scheduler
{
    public class SchedulerManager
    {
        static readonly Lazy<SchedulerManager> lazy = new Lazy<SchedulerManager>(() => { return new SchedulerManager(); });
        static IScheduler scheduler;
        private SchedulerManager()
        {
            scheduler = new StdSchedulerFactory().GetScheduler().Result;
            scheduler.Start().Wait();
        }
        public static SchedulerManager Default
        {
            get
            {
                return lazy.Value;
            }
        }
        public void AddJob(Type type, string cronExpression, params KeyValuePair<string, string>[] keyValue)
        {
            JobKey jobKey = new JobKey(type.FullName + Cryptor.EncryptMD5(cronExpression));
            var job = scheduler.GetJobDetail(jobKey).Result;
            if (job != null)
            {
                var t = TriggerBuilder.Create().WithIdentity(type.Name, type.Namespace).WithSimpleSchedule(m => m.RepeatForever().WithRepeatCount(0).WithIntervalInSeconds(1)).Build();
                scheduler.ScheduleJob(job, t);
                return;
            }
            var builder = JobBuilder.Create(type).WithIdentity(jobKey);
            foreach (var item in keyValue)
            {
                builder.UsingJobData(item.Key, item.Value);
            }
            job = builder.Build();

            TriggerKey triggerKey = new TriggerKey(cronExpression);
            var trigger = scheduler.GetTrigger(triggerKey).Result;
            if (trigger == null)
            {
                trigger = TriggerBuilder.Create()
                    .WithIdentity(cronExpression)
                    .StartNow()
                    .WithCronSchedule(cronExpression)
                    .Build();
            }
            scheduler.ScheduleJob(job, trigger).Wait();
            scheduler.Start().Wait();
        }

     
        public void AddJob<TJob>(string cronExpression, params KeyValuePair<string, string>[] keyValue)
        {
            var type = typeof(TJob);
            AddJob(type, cronExpression, keyValue);
        }
        public List<JobDescription> Stop()
        {
            List<JobDescription> list = new List<JobDescription>();
            var jobGroups = scheduler.GetJobGroupNames().Result;
            foreach (string group in jobGroups)
            {
                var groupMatcher = GroupMatcher<JobKey>.GroupContains(group);
                var jobKeys = scheduler.GetJobKeys(groupMatcher).Result;
                foreach (var jobKey in jobKeys)
                {
                    var detail = scheduler.GetJobDetail(jobKey).Result;
                    var triggers = scheduler.GetTriggersOfJob(jobKey).Result;
                    foreach (ITrigger trigger in triggers)
                    {
                        list.Add(new JobDescription()
                        {
                            Group = group,
                            Name = jobKey.Name,
                            Description = detail.Description,
                            TriggerName = trigger.Key.Name,
                            TriggerType = trigger.GetType().Name,
                            TriggerState = scheduler.GetTriggerState(trigger.Key).Result,
                            NextFireTime = trigger.GetNextFireTimeUtc(),
                            PreviousFireTime = trigger.GetPreviousFireTimeUtc(),
                        });



                    }
                }
            }
            return list;
        }
    }
}