using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.IO.Compression;

namespace Bzway.Common.Utility
{
    public class FileManager
    {
        Dictionary<uint, SuperBlock> superBlocks = new Dictionary<uint, SuperBlock>(10);
        public FileManager()
        {
            var superBlock = new SuperBlock(1, Directory.GetCurrentDirectory() + "/dir");
            this.superBlocks.Add(1, superBlock);
        }
        public void AddFile(string fileName, string contentType, Stream stream, long length, string path = "/")
        {
            this.superBlocks[1].AddFile(fileName, stream, length, contentType, path);
            this.superBlocks[1].Write();
        }
        /// <summary>
        /// volumneid,fileid,cookies,subkey
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public Stream GetFile(string fileId)
        {
            var list = fileId.Split(',');
            var volumnId = uint.Parse(list[0]);
            return this.superBlocks[volumnId].GetFile(list[1], list[2], 0);
        }
        public void Recovery()
        {
            this.superBlocks[1].Recovery();
        }
    }
}