using Quartz;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Bzway.Framework.Scheduler
{
    public class AssemblyJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var jobDataMap = context.MergedJobDataMap;
                var job = JobAssemblyInfo.GetAssemblyJob(jobDataMap);
                if (job == null)
                {
                    return;
                }
                var jobName = context.JobDetail.Key.Name;
                var jobGroupName = context.JobDetail.Key.Group;
                var jobScheduleName = context.Trigger.Key.Name;
                var startTime = DateTime.Now;
                long periodBySecond;
                var endTime = DateTime.Now;

                try
                {
                    job.Invoke( );
                    stopwatch.Stop();
                    periodBySecond = stopwatch.ElapsedMilliseconds;
                    endTime = DateTime.Now;
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    periodBySecond = stopwatch.ElapsedMilliseconds;
                    endTime = DateTime.Now;
                }
                finally
                {

                }
            });
        }
    }
}