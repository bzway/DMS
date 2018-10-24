using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Bzway.Framework.DistributedFileSystemClient.Core;
using Bzway.Framework.DistributedFileSystemClient.Core.File;
using Bzway.Framework.DistributedFileSystemClient.Core.Http;
using Bzway.Framework.DistributedFileSystemClient.Exception;

namespace Bzway.Framework.DistributedFileSystemClient
{
    /// <summary>
    /// Seaweed file system operation template.
    /// </summary>
    public class OperationsTemplate : IOperationsTemplate
    {
        private readonly MasterWrapper _masterWrapper;
        private readonly VolumeWrapper _volumeWrapper;
        private int _sameRackCount;
        private int _diffRackCount;
        private int _diffDataCenterCount;
        private string _timeToLive;
        private string _dataCenter;
        private string _collection;
        private AssignFileKeyParams _fileKeyParams;
        private string _replicationFlag;

        public OperationsTemplate(IMasterConnection master)
        {
            _masterWrapper = new MasterWrapper(master.GetConnection());
            _volumeWrapper = new VolumeWrapper(master.GetConnection());
            _replicationFlag = "000";
            _fileKeyParams = new AssignFileKeyParams();
            UsingPublicUrl = true;
            LoadBalance = true;
        }

        public int SameRackCount
        {
            get { return _sameRackCount; }
            set
            {
                if (value < 0 || value > 9)
                    throw new ArgumentOutOfRangeException("CountOutOfRange");
                _sameRackCount = value;
                BuildReplicationFlag();
                BuildAssignFileKeyParams();
            }
        }

        public int DiffRackCount
        {
            get
            {
                return _diffRackCount;
            }
            set
            {
                if (value < 0 || value > 9)
                    throw new ArgumentOutOfRangeException("CountOutOfRange");
                _diffRackCount = value;
                BuildReplicationFlag();
                BuildAssignFileKeyParams();
            }
        }

        public int DiffDataCenterCount
        {
            get
            {
                return _diffDataCenterCount;
            }
            set
            {
                if (value < 0 || value > 9)
                    throw new ArgumentOutOfRangeException("CountOutOfRange");
                _diffDataCenterCount = value;
                BuildReplicationFlag();
                BuildAssignFileKeyParams();
            }
        }

        public string TTL
        {
            get { return _timeToLive; }
            set
            {
                _timeToLive = value.Trim();
                BuildAssignFileKeyParams();
            }
        }

        public string DataCenter
        {
            get
            {
                return _dataCenter;
            }
            set
            {
                _dataCenter = value;
                BuildAssignFileKeyParams();
            }
        }

        public string Collection
        {
            get
            {
                return _collection;
            }
            set
            {
                _collection = value;
                BuildAssignFileKeyParams();
            }
        }

        public bool UsingPublicUrl { get; set; }
        public bool LoadBalance { get; set; }

        /// <summary>
        /// Save a file.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task<FileHandleStatus> SaveFileByStream(string fileName, Stream stream)
        {
            var fileKeyResult = await _masterWrapper.AssignFileKey(_fileKeyParams);
            var uploadUrl = UsingPublicUrl ? fileKeyResult.PublicUrl : fileKeyResult.Url;
            // Upload file
            return await _volumeWrapper.UploadFile(uploadUrl, fileKeyResult.FileId, fileName, stream, TTL);
        }

        /// <summary>
        /// Save files by stream map.
        /// </summary>
        /// <param name="streamMap"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, FileHandleStatus>> SaveFilesByStreamMap(Dictionary<string, Stream> streamMap)
        {
            // Assign file key
            var values = new AssignFileKeyParams(
                _fileKeyParams.Replication,
                streamMap.Count,
                _fileKeyParams.DataCenter,
                _fileKeyParams.TTL,
                _fileKeyParams.Collection
            );
            var fileKeyResult = await _masterWrapper.AssignFileKey(values);
            var uploadUrl = UsingPublicUrl ? fileKeyResult.PublicUrl : fileKeyResult.Url;
            // Upload file
            var index = 1;
            var resultMap = new Dictionary<string, FileHandleStatus>(streamMap.Count);
            using (var enumerator = streamMap.GetEnumerator())
            {
                enumerator.MoveNext();
                var streamItem = enumerator.Current;
                resultMap.Add(streamItem.Key, await _volumeWrapper.UploadFile(uploadUrl, fileKeyResult.FileId, streamItem.Key, streamItem.Value, TTL));

                while (enumerator.MoveNext())
                {
                    streamItem = enumerator.Current;
                    resultMap.Add(streamItem.Key, await _volumeWrapper.UploadFile(uploadUrl, fileKeyResult.FileId + "_" + index++, streamItem.Key, streamItem.Value, TTL));
                }
            }
            return resultMap;
        }

        /// <summary>
        /// Delete file.
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteFile(string fileId)
        {
            var targetUrl = await GetTargetUrl(fileId);
            var result = await _volumeWrapper.CheckFileExist(targetUrl, fileId);
            if (result)
                await _volumeWrapper.DeleteFile(targetUrl, fileId);
            return result;
        }

        /// <summary>
        /// Delete files.
        /// </summary>
        /// <param name="fileIds"></param>
        /// <returns></returns>
        public async Task DeleteFiles(IList<string> fileIds)
        {
            var tasks = new Task[fileIds.Count];
            var index = 0;
            foreach (var fileId in fileIds)
            {
                var task = DeleteFile(fileId);
                tasks[index++] = task;
            }
            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Update file, if file id is not exist, it wouldn't throw any exception.
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="fileName"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task<FileHandleStatus> UpdateFileByStream(string fileId, string fileName, Stream stream)
        {
            var targetUrl = await GetTargetUrl(fileId);
            await _volumeWrapper.CheckFileExist(targetUrl, fileId);
            return await _volumeWrapper.UploadFile(targetUrl, fileId, fileName, stream, TTL);
        }

        /// <summary>
        /// Get file stream, this is the faster method to get file stream from server.
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public async Task<StreamResponse> GetFileStream(string fileId)
        {
            var targetUrl = await GetTargetUrl(fileId);
            return await _volumeWrapper.GetFileStream(targetUrl, fileId);
        }

        /// <summary>
        /// Get file status without file stream.
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public async Task<FileHandleStatus> GetFileStatus(string fileId)
        {
            var targetUrl = await GetTargetUrl(fileId);
            var headerResponse = await _volumeWrapper.GetFileStatusHeader(targetUrl, fileId);
            try
            {
                var cpString = headerResponse.GetLastHeader("Content-Disposition");
                var filename = ContentDispositionHeaderValue
                    .Parse(cpString)
                    .FileName
                    .Trim('"');
                long fileSize;
                if (!long.TryParse(headerResponse.GetLastHeader("Content-Length"), out fileSize))
                {
                    fileSize = 0;
                }

                return new FileHandleStatus()
                {
                    ContentType = headerResponse.GetLastHeader("Content-Type"),
                    FileId = fileId,
                    LastModified = DateTime.Parse(headerResponse.GetLastHeader("Last-Modified"), CultureInfo.InvariantCulture).Ticks,
                    Name = filename,
                    Size = fileSize,
                };
            }
            catch (FormatException)
            {
                throw new SeaweedFsException(string.Format("FileStatusParseException:{0}", headerResponse.GetLastHeader("Last-Modified")));
            }
            catch (NullReferenceException)
            {
                throw new SeaweedFsException("FileDoesNotExist");
            }
        }

        public async Task<bool> CheckFileExists(string fileId)
        {
            var targetUrl = await GetTargetUrl(fileId);
            return await _volumeWrapper.CheckFileExist(targetUrl, fileId);
        }

        /// <summary>
        /// Get file url, could get file directly from seaweedfs volume server.
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public async Task<string> GetFileUrl(string fileId)
        {
            var targetUrl = await GetTargetUrl(fileId);
            return targetUrl + "/" + fileId;
        }

        private void BuildReplicationFlag()
        {
            _replicationFlag = string.Concat(_diffDataCenterCount,
                _diffRackCount, _sameRackCount);
        }

        private void BuildAssignFileKeyParams()
        {
            var value = new AssignFileKeyParams(_replicationFlag, 1, _dataCenter, _timeToLive, _collection);
            _fileKeyParams = value;
        }

        private async Task<LocationResult> GetTargetLocation(string fileId)
        {
            var result = await _masterWrapper.LookupVolume(new LookupVolumeParams(fileId, Collection));
            if (result.Locations == null || result.Locations.Count == 0)
                throw new SeaweedFsFileDeleteException(fileId,
                    new SeaweedFsException("VolumeServerNotFound"));
            return LoadBalance ? result.GetRandomLocation() : result.Locations[0];
        }

        private async Task<string> GetTargetUrl(string fileId)
        {
            var result = await GetTargetLocation(fileId);
            return UsingPublicUrl ? result.PublicUrl : result.Url;
        }
    }
}
