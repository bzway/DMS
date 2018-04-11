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
    public class SqlServerJob : IJob
    {
        private const string TargetCommandType = "TargetCommandType";
        private const string TargetCommandText = "TargetCommandText";
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                var jobDataMap = context.MergedJobDataMap;
                var sqlConnection = jobDataMap.GetString(TargetCommandType);
                var targetCommandText = jobDataMap.GetString(TargetCommandText);
                var jobName = context.JobDetail.Key.Name;
                var jobGroupName = context.JobDetail.Key.Group;
                var jobScheduleName = context.Trigger.Key.Name;
                var jobScheduleGroupName = context.Trigger.Key.Group;
                var startTime = DateTime.Now;
                long periodBySecond = 0;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                try
                {
                    IDbConnection cnn = new SqlConnection(sqlConnection);
                    Dapper.SqlMapper.ExecuteScalar(cnn, targetCommandText);
                    stopwatch.Stop();
                    periodBySecond = stopwatch.ElapsedMilliseconds;
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    stopwatch.Stop();
                    periodBySecond = stopwatch.ElapsedMilliseconds;
                }
                finally
                {
                }
            });
        }
    }
}