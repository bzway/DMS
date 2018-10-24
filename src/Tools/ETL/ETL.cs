using Bzway.Common.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.XSSF.UserModel;
using System.Data;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using Bzway.Common.Script;
using System.Reflection;
using Bzway.Database.Core;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;
using System.Runtime.Loader;
using System.Data.SqlClient;
using System.Threading;
using Bzway.Tools.Grok.Core;
using System.Text.RegularExpressions;

namespace Bzway.Tools.ETL
{
    public class ETL
    {
        #region ctor
        static ETL _default;
        Dictionary<string, IInputProvider> InputProviders;
        Dictionary<string, IOutputProvider> OutputProviders;
        Dictionary<string, ITransferProvider> TransferProviders;
        ETL()
        {
            this.InputProviders = new Dictionary<string, IInputProvider>();
            this.OutputProviders = new Dictionary<string, IOutputProvider>();
            this.TransferProviders = new Dictionary<string, ITransferProvider>();
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                foreach (var type in assembly.GetTypes().Where(m => typeof(IInputProvider).IsAssignableFrom(m) && m.IsClass))
                {
                    var o = (IInputProvider)assembly.CreateInstance(type.FullName);
                    this.InputProviders.Add(o.Name, o);
                }
                foreach (var type in assembly.GetTypes().Where(m => typeof(IOutputProvider).IsAssignableFrom(m) && m.IsClass))
                {
                    var o = (IOutputProvider)assembly.CreateInstance(type.FullName);
                    this.OutputProviders.Add(o.Name, o);
                }
                foreach (var type in assembly.GetTypes().Where(m => typeof(ITransferProvider).IsAssignableFrom(m) && m.IsClass))
                {
                    var o = (ITransferProvider)assembly.CreateInstance(type.FullName);
                    this.TransferProviders.Add(o.Name, o);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Load Providers Error:" + ex.Message);
            }
        }
        public static ETL Instance
        {
            get
            {
                if (_default == null)
                {
                    _default = new ETL();
                }
                return _default;
            }
        }

        #endregion

        public void Do(string path)
        {
            try
            {
                JObject data = (JObject)JsonConvert.DeserializeObject(File.ReadAllText(path));
                while (true)
                {
                    var input = Input((JObject)data["input"]);
                    var list = this.Process((JObject)data["transfer"], input);
                    var output = Output((JObject)data["output"], list);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Do ETL Error:" + ex.Message);
            }
        }
        List<DynamicEntity> Input(JObject setting)
        {

            try
            {
                var type = setting["type"].Value<string>();
                var list = this.InputProviders[type].Process(setting);
                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Do Input Error:" + ex.Message);
                return new List<DynamicEntity>();
            }
        }
        List<object> Process(JObject setting, List<DynamicEntity> input)
        {
            try
            {
                var type = setting["type"].Value<string>();
                var list = this.TransferProviders[type].Process(setting, input);
                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Do Output Error:" + ex.Message);
                return new List<object>();
            }
        }
        string Output(JObject setting, List<object> list)
        {
            try
            {
                var type = setting["type"].Value<string>();
                return this.OutputProviders[type].Process(setting, list);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Do Output Error:" + ex.Message);
                return ex.Message;
            }
        }
    }

    public interface ITransferProvider
    {
        string Name { get; }
        List<object> Process(JObject setting, List<DynamicEntity> input);
    }
    public interface IInputProvider
    {
        string Name { get; }
        List<DynamicEntity> Process(JObject setting);
    }
    public interface IOutputProvider
    {
        string Name { get; }
        string Process(JObject setting, List<object> list);
    }
    public class MapTransferProvider : ITransferProvider
    {
        public string Name => "map";
        public List<object> Process(JObject setting, List<DynamicEntity> input)
        {
            var code = setting["map"].Value<string>();// "row => new { Name = row.name, Age = row.name.ToString() + row.age.ToString()}"
            var convert = Generator.GenerateDynamicProcess(code);
            return input.Select(convert).ToList();
        }
    }
    public class GrokTransferProvider : ITransferProvider
    {
        public string Name => "grok";
        public List<object> Process(JObject setting, List<DynamicEntity> input)
        {
            List<object> list = new List<object>();
            var grok = setting["grok"].Value<string>();

            var resolver = new RegexAliasResolver(BasicAliasConfigReader.Parse(Path.Combine(AppContext.BaseDirectory, "grok.patterns")));
            var regex = resolver.ResolveToRegex(grok);
            foreach (dynamic data in input)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();

                foreach (Match match in regex.Matches(data.message))
                {
                    foreach (var item in match.Groups.Skip(1).Where(m => m.Success))
                    {
                        dict.Add(item.Name, item.Value);
                    }
                }
                list.Add(new DynamicEntity(dict));
            }
            return list;
        }
    }

    public class FileInputProvider : IInputProvider
    {
        public string Name => "file";

        public List<DynamicEntity> Process(JObject setting)
        {
            var path = setting["path"].Value<string>();
            if (path.EndsWith("csv") || path.EndsWith("txt"))
            {
                return SourceReader.ReadCsv(path);
            }
            else
            {
                return SourceReader.ReadExcel(path);
            }
        }
    }

    public class LogInputProvider : IInputProvider
    {
        public string Name => "log";

        public List<DynamicEntity> Process(JObject setting)
        {
            var path = setting["path"].Value<string>();
            var maxRecord = setting["maxRecord"].Value<int>();

            return SourceReader.ReadLog(path, maxRecord);
        }
    }

    public class SQLInputProvider : IInputProvider
    {
        public string Name => "sqlserver";
        public List<DynamicEntity> Process(JObject setting)
        {
            try
            {
                var connectionString = setting["connectionString"].Value<string>();
                var sql = setting["sql"].Value<string>();
                return SourceReader.ReadSQLServer(connectionString, sql, 100000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Do sqlserver Input Error:" + ex.Message);
                return new List<DynamicEntity>();
            }
        }
    }
    public class MongodbOutputProvider : IOutputProvider
    {
        public string Name => "mongodb";

        public string Process(JObject setting, List<object> list)
        {
            try
            {
                var connection = setting["connection"].Value<string>();
                var database = setting["database"].Value<string>();
                var table = setting["table"].Value<string>();
                var mongoClient = new MongoClient(connection);
                var db = mongoClient.GetDatabase(database);
                var collection = db.GetCollection<BsonDocument>(table);
                foreach (var item in list)
                {
                    if (item.GetType() == typeof(DynamicEntity))
                    {
                        collection.InsertOne(((DynamicEntity)item).ToBsonDocument());
                    }
                    else
                    {
                        collection.InsertOne(item.ToBsonDocument());
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
    public class SqlOutputProvider : IOutputProvider
    {
        public string Name => "sql";

        public string Process(JObject setting, List<object> list)
        {
            try
            {
                var connection = setting["connection"].Value<string>();
                var database = setting["database"].Value<string>();
                var table = setting["table"].Value<string>();
                var mongoClient = new MongoClient(connection);
                var db = mongoClient.GetDatabase(database);
                var collection = db.GetCollection<BsonDocument>(table);
                foreach (var item in list)
                {
                   
                    if (item.GetType() == typeof(DynamicEntity))
                    {
                        collection.InsertOne(((DynamicEntity)item).ToBsonDocument());
                    }
                    else
                    {
                        collection.InsertOne(item.ToBsonDocument());
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}