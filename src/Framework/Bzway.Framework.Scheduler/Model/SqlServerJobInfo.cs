using Quartz;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace Bzway.Framework.Scheduler
{
    public class SqlServerJobInfo : IJobInfo
    {
        #region ctor
        static Dictionary<string, IJobInfo> dictionary = new Dictionary<string, IJobInfo>();
        private static readonly string sqlConnectionStringKey = "sqlconnectionstring";
        private static readonly string sqlCommandKey = "sqlcommand";
        private static readonly string[] settings = new string[] { sqlConnectionStringKey, sqlCommandKey };

        private readonly IDbConnection connection;
        private readonly string sqlCommand;
        private readonly JobDataMap jobDataMap;
        public SqlServerJobInfo()
        {
        }
        public SqlServerJobInfo(IDbConnection cnn, string sqlCommand, JobDataMap jobDataMap)
        {
            this.connection = cnn;
            this.sqlCommand = sqlCommand;
            this.jobDataMap = jobDataMap;
        }


        #endregion
        public static IJobInfo GetAssemblyJob(JobDataMap jobDataMap)
        {
            var sqlConnection = jobDataMap.GetString(sqlConnectionStringKey);
            if (string.IsNullOrEmpty(sqlConnection))
            {
                return null;
            }
            var sqlCommand = jobDataMap.GetString(sqlCommandKey);
            if (string.IsNullOrEmpty(sqlCommand))
            {
                return null;
            }
            string key = string.Format("{0}_{1}", sqlConnection, sqlCommand);
            if (!dictionary.ContainsKey(key))
            {
                var obj = new SqlServerJobInfo(new SqlConnection(sqlConnection), sqlCommand, jobDataMap);
                dictionary.Add(key, obj);
            }
            return dictionary[key];
        }
        public object Invoke()
        {
            return SqlMapper.ExecuteScalar(connection, sqlCommand);
        }
        public string[] Settings => settings;
    }
}