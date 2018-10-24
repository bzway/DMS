﻿/*
 * Original work Copyright (c) 2016 Lokra Studio
 * Url: https://github.com/lokra/seaweedfs-client
 * Modified work Copyright 2017 smartbox software solutions
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Bzway.Framework.DistributedFileSystemClient.Core.Http;
using Bzway.Framework.DistributedFileSystemClient.Exception;
using Bzway.Framework.DistributedFileSystemClient.Core.File;

namespace Bzway.Framework.DistributedFileSystemClient.Core
{
    public class VolumeWrapper
    {
        private readonly Connection _connection;

        public VolumeWrapper(Connection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Upload file.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileId"></param>
        /// <param name="filename"></param>
        /// <param name="inputStream"></param>
        /// <param name="ttl"></param>
        /// <returns></returns>
        public async Task<FileHandleStatus> UploadFile(string url, string fileId, string filename, Stream inputStream, string ttl)
        {
            url = url + "/" + fileId;
            if (!string.IsNullOrEmpty(ttl))
            {
                url = url + "?ttl=" + ttl;
            }
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri(url));

            JsonResponse jsonResponse;
            using (var content = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture)))
            {
                content.Add(new StreamContent(inputStream), Path.GetFileNameWithoutExtension(filename), filename);
                request.Content = content;
                jsonResponse = await _connection.FetchJsonResultByRequest(request);
            }

            ConvertResponseStatusToException((int)jsonResponse.StatusCode, url, fileId, false, false, false, false);

            var result = JsonConvert.DeserializeObject<FileHandleStatus>(jsonResponse.Json);
            result.LastModified = DateTime.UtcNow.ToFileTimeUtc();
            result.FileId = fileId;
            return result;
        }


        /// <summary>
        /// Delete file.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileId"></param>
        public async Task<bool> DeleteFile(string url, string fileId)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Delete,
                new Uri(url + "/" + fileId)
            );
            var jsonResponse = await _connection.FetchJsonResultByRequest(request);
            try
            {
                ConvertResponseStatusToException((int)jsonResponse.StatusCode, url, fileId, false, false, false, false);
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        /// <summary>
        /// Check file exists.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileId"></param>
        public async Task<bool> CheckFileExist(string url, string fileId)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Head,
                new Uri(url + "/" + fileId)
            );
            var statusCode = await _connection.FetchStatusCodeByRequest(request);
            try
            {
                ConvertResponseStatusToException((int)statusCode, url, fileId, false, true, false, false);
                return true;
            }
            catch (SeaweedFsFileNotFoundException)
            {
                return false;
            }
        }

        /// <summary>
        /// Get file stream.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public async Task<StreamResponse> GetFileStream(string url, string fileId)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                new Uri(url + "/" + fileId)
            );
            var result = await _connection.FetchStreamByRequest(request);
            ConvertResponseStatusToException((int)result.StatusCode, url, fileId, false, false, false, false);
            return result;
        }

        /// <summary>
        /// Get file status.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public async Task<HeaderResponse> GetFileStatusHeader(string url, string fileId)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Head,
                new Uri(url + "/" + fileId)
            );
            var response = await _connection.FetchHeaderByRequest(request);
            ConvertResponseStatusToException((int)response.StatusCode, url, fileId, false, false, false, false);
            return response;
        }

        /// Note: This void has to be non static
        private void ConvertResponseStatusToException(int statusCode, string url, string fileId,
            bool ignoreNotFound, bool ignoreRedirect, bool ignoreRequestError, bool ignoreServerError)
        {
            switch (statusCode / 100)
            {
                case 1:
                case 2:
                    return;
                case 3:
                    if (ignoreRedirect)
                        return;
                    throw new SeaweedFsException(string.Format("FetchFileRedirectError:url:{0}, fileId:{1}status:{2}", url, fileId, statusCode));
                case 4:
                    if (statusCode == 404 && ignoreNotFound)
                        return;
                    if (statusCode == 404)
                        throw new SeaweedFsFileNotFoundException(string.Format("FetchFileNotFoundError:url:{0}, fileId:{1}status:{2}", url, fileId, statusCode));
                    if (ignoreRequestError)
                        return;
                    throw new SeaweedFsException(string.Format("FetchFileRequestError:url:{0}, fileId:{1}status:{2}", url, fileId, statusCode));
                case 5:
                    if (ignoreServerError)
                        return;
                    throw new SeaweedFsException(string.Format("FetchFileRequestError:url:{0}, fileId:{1}status:{2}", url, fileId, statusCode));
                default:
                    throw new SeaweedFsException(string.Format("FetchFileRequestError:url:{0}, fileId:{1}status:{2}", url, fileId, statusCode));
            }
        }
    }
}
