using Quartz;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace Bzway.Framework.Scheduler
{
    public class JobAssemblyInfo : IJobInfo
    {
        #region ctor
        static Dictionary<string, IJobInfo> dictionary = new Dictionary<string, IJobInfo>();
        private static readonly string TargetAssemblyPath = "TargetAssemblyPath";
        private static readonly string TargetTypeName = "TargetTypeName";
        private static readonly string TargetMethodName = "TargetMethodName";
        private static readonly string[] settings = new string[] { TargetAssemblyPath, TargetTypeName, TargetMethodName };

        private readonly Type type;
        private readonly object my;
        private readonly MethodInfo method;
        private readonly ParameterInfo[] paras;
        private readonly JobDataMap jobDataMap;

        public readonly string ErrorMessage;

        public JobAssemblyInfo()
        {
        }
        public JobAssemblyInfo(Type type, string targetMethodName, JobDataMap jobDataMap)
        {
            this.jobDataMap = jobDataMap;
            try
            {
                this.type = type;
                this.my = Activator.CreateInstance(this.type);
                this.method = type.GetMethod(targetMethodName);
                this.paras = this.method.GetParameters();
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }

        #endregion
        public static IJobInfo GetAssemblyJob(JobDataMap jobDataMap)
        {
            var targetAssemblyPath = jobDataMap.GetString(TargetAssemblyPath);
            if (targetAssemblyPath == null)
            {
                return null;
            }
            var targetTypeFullName = jobDataMap.GetString(TargetTypeName);
            if (targetTypeFullName == null)
            {
                return null;
            }
            var targetMethodName = jobDataMap.GetString(TargetMethodName);
            if (string.IsNullOrEmpty(targetMethodName))
            {
                return null;
            }

            string key = string.Format("{0}_{1}_{2}", targetAssemblyPath, targetTypeFullName, targetMethodName);
            if (!dictionary.ContainsKey(key))
            {
                if (!File.Exists(targetAssemblyPath))
                {
                    targetAssemblyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, targetAssemblyPath);
                }
                if (!File.Exists(targetAssemblyPath))
                {
                    return null;
                }
                var assembly = Assembly.LoadFrom(targetAssemblyPath);
                if (assembly == null)
                {
                    return null;
                }

                var type = assembly.GetType(targetTypeFullName);
                if (type == null)
                {
                    targetTypeFullName = assembly.FullName.Split(',')[0] + "." + targetTypeFullName;
                }
                type = assembly.GetType(targetTypeFullName);
                if (type == null)
                {
                    return null;
                }
                var obj = new JobAssemblyInfo(type, targetMethodName, jobDataMap);
                if (!string.IsNullOrEmpty(obj.ErrorMessage))
                {
                    return null;
                }
                dictionary.Add(key, obj);
            }
            return dictionary[key];
        }
        public object Invoke()
        {
            var args = new object[paras.Length];
            for (int i = 0; i < paras.Length; i++)
            {
                args[i] = jobDataMap.Get(paras[i].Name);
            }
            return method.Invoke(this.my, args);
        }
        public string[] Settings => settings;
    }
}