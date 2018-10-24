using Bzway.Database.Core;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Bzway.Framework.Application;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System.Security.Principal;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using Bzway.Common.Share;
using Bzway.Framework.Application.Entity;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Bzway.Framework.DistributedFileSystemClient.Core.Http;
using Bzway.Framework.DistributedFileSystemClient.Core;

namespace Bzway.Framework.DistributedFileSystemClient
{
    /// <summary>
    /// GrantRequest service
    /// </summary>
    public partial class DistributedFileSystemServiceService : BaseService<DistributedFileSystemServiceService>, IDistributedFileSystemService
    {

        #region ctor
        readonly ISiteService siteService;
        readonly MasterConnection master;
        readonly IConfigurationSection redisConfig;
        public DistributedFileSystemServiceService(ISiteService siteService, MasterConnection master, IConfiguration configuration, ILoggerFactory loggerFactory, ITenant tenant) : base(loggerFactory, tenant)
        {
            this.siteService = siteService;
            this.master = master;
            this.redisConfig = configuration.GetSection("Redis");
        }
        #endregion


        public DirectoryContents ListDirectoryContent(string directoryPath)
        {
            var segments = directoryPath.Split('/', '\\').Where(m => !string.IsNullOrEmpty(m)).ToArray();
            var key = string.Join("/", "FilePath:" + this.tenant.Site.Id, segments);
            using (ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(this.redisConfig.GetValue<string>("RedisConfiguration", "localhost")))
            {
                var db = connection.GetDatabase();
                var result = db.SetMembers(key);
                var files = result.Select(m =>
                {
                    var fileInfo = db.StringGet(key + "/" + m);
                    if (fileInfo.HasValue)
                    {
                        return JsonConvert.DeserializeObject<DistributedFileInfo>(fileInfo);
                    }
                    return new DistributedFileInfo() { IsDirectory = true, ContentType = "zip", FileId = "", Name = m, };
                });

                return new DirectoryContents(files);
            }
        }
        public DistributedFileInfo CreateOrUpdateFile(string filePath, Stream stream)
        {
            var segments = filePath.Split('/', '\\').Where(m => !string.IsNullOrEmpty(m)).ToArray();
            var key = string.Join("/", "FilePath:" + this.tenant.Site.Id, segments);

            using (ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(this.redisConfig.GetValue<string>("RedisConfiguration", "localhost")))
            {
                var db = connection.GetDatabase();
                if (!db.KeyExists(key))
                {
                    //如果文件没有映射，创建目录结构
                    CreateDirectory(key, db);
                }
                //保存文件得到文件元数据
                var template = new OperationsTemplate(master);
                var result = template.SaveFileByStream(segments.Last(), stream).Result;

                DistributedFileInfo fileInfo = new DistributedFileInfo()
                {
                    IsDirectory = false,
                    Length = result.Size,
                    LastModified = DateTimeOffset.FromUnixTimeSeconds(result.LastModified),
                    Name = result.Name,
                    FileId = result.FileId,
                    ContentType = result.ContentType,
                    Url = template.GetFileUrl(result.FileId).Result,
                };
                var value = JsonConvert.SerializeObject(fileInfo);
                db.StringSet(key, value);
                return fileInfo;
            }
        }

        private void CreateDirectory(string key, IDatabase db)
        {
            var paths = key.Split('/');
            var dirPath = string.Empty;

            for (int i = 0; i < paths.Length - 1; i++)
            {
                dirPath = "/" + paths[i];
                if (!db.SetContains(dirPath, paths[i + 1]))
                {
                    db.SetAdd(dirPath, paths[i + 1]);
                }
            }
        }

        public DistributedFileStream GetFileStream(string filePath)
        {
            var segments = filePath.Split('/', '\\').Where(m => !string.IsNullOrEmpty(m)).ToArray();
            var key = string.Join("/", "FilePath:" + this.tenant.Site.Id, segments);

            using (ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(this.redisConfig.GetValue<string>("RedisConfiguration", "localhost")))
            {
                var db = connection.GetDatabase();
                var fileInfo = db.StringGet(key);
                if (fileInfo.HasValue)
                {
                    var file = JsonConvert.DeserializeObject<DistributedFileInfo>(fileInfo);
                    var template = new OperationsTemplate(master);
                    return new DistributedFileStream() { Info = file, Stream = template.GetFileStream(file.FileId).Result };
                }
                return null;
            }

        }
    }
}