using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.IO.Compression;

namespace Bzway.Common.Utility
{
    public class SuperBlock
    {
        List<BlockIndex> fileBlocks = new List<BlockIndex>();
        Dictionary<string, int> fileNameIndex = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
        Dictionary<string, int> fileDataIndex = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
        Dictionary<uint, int> fileKeyIndex = new Dictionary<uint, int>();
        private string dataFile;
        private string indexFile;
        public uint VolumeId { get; set; }
        public bool ReadOnly { get; set; }
        public int TTL { get; set; }

        private static object lockObject = new object();

        private FileStream dataFileStream;
        public SuperBlock(uint volumeId, string root)
        {
            this.VolumeId = volumeId;
            this.dataFile = Path.Combine(root, this.VolumeId + ".data");
            this.indexFile = Path.Combine(root, this.VolumeId + ".index");
            this.dataFileStream = File.Open(this.dataFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BuildIndex();
        }

        private int ReadInt(Stream indexFileReader)
        {
            byte[] buffer = new byte[4];
            indexFileReader.Read(buffer, 0, 4);
            int size = BitConverter.ToInt32(buffer, 0);
            return size;
        }
        private void BuildIndex()
        {
            using (var indexFileReader = File.Open(this.indexFile, FileMode.OpenOrCreate, FileAccess.Read))
            {

                while (true)
                {
                    var size = ReadInt(indexFileReader);
                    if (size <= 0)
                    {
                        return;
                    }
                    var buffer = new byte[size];
                    indexFileReader.Read(buffer, 0, size);

                    var index = BlockIndex.ReadBlockIndex(buffer);
                    //判断文件内容是否已经存在
                    if (fileDataIndex.ContainsKey(index.Hash))
                    {
                        //判断文件名称是否已经存在
                        if (!fileNameIndex.ContainsKey(index.FileName))
                        {
                            fileNameIndex.Add(index.FileName, fileDataIndex[index.Hash]);
                        }
                        else
                        {
                            fileNameIndex[index.FileName] = fileDataIndex[index.Hash];
                        }
                        continue;
                    }
                    this.fileBlocks.Add(index);
                    var fileIndex = this.fileBlocks.Count - 1;
                    fileDataIndex.Add(index.Hash, fileIndex);
                    fileNameIndex.Add(index.FileName, fileIndex);
                    
                }
            }
        }


        public void Dispose()
        {
            this.dataFileStream.Close();
        }
        public void AddFile(string fileName, Stream stream, long length, string contentType, string path)
        {

            //得到文件的数据
            byte[] dataBytes = GetCompressedData(stream);
            //得到文件的名称
            var filePath = path + "/" + fileName;
            //得到文件的哈希
            var hash = Cryptor.EncryptSHA1(dataBytes);
            //判断文件内容是否已经存在
            if (fileDataIndex.ContainsKey(hash))
            {
                //判断文件名称是否已经存在
                if (!fileNameIndex.ContainsKey(filePath))
                {
                    fileNameIndex.Add(filePath, fileDataIndex[hash]);
                }
                else
                {
                    fileNameIndex[filePath] = fileDataIndex[hash];
                }
                return;
            }
            var blockIndex = new BlockIndex((uint)(dataFileStream.Position / 8), filePath, hash);
            this.fileBlocks.Add(blockIndex);
            var fileIndex = this.fileBlocks.Count - 1;
            fileDataIndex.Add(hash, fileIndex);
            //判断文件名称是否已经存在
            if (!fileNameIndex.ContainsKey(filePath))
            {
                fileNameIndex.Add(filePath, fileDataIndex[hash]);
            }
            else
            {
                fileNameIndex[filePath] = fileDataIndex[hash];
            }
            Block block = new Block(blockIndex.Offset, blockIndex.FileName, blockIndex.Hash, contentType, 0, dataBytes, blockIndex.Cookie);
            dataBytes = block.ToBytes();
            dataFileStream.Write(dataBytes, 0, dataBytes.Length);
            dataFileStream.Flush();
        }
        public Stream GetFile(string fileName, string path, uint cookies)
        {
            //得到文件的名称
            var filePath = path + "/" + fileName;

            if (this.fileNameIndex.ContainsKey(filePath))
            {
                var blockIndex = fileBlocks[this.fileNameIndex[filePath]];
                if (blockIndex.Cookie != cookies)
                {
                    return null;
                }
                if (blockIndex.Flag == 0 || blockIndex.TTL != 0)
                {
                    return null;
                }
                this.dataFileStream.Position = blockIndex.Offset * 8;
                Block block = Block.ReadBlock(this.dataFileStream);
                return new MemoryStream(block.Data);
            }
            throw new NotImplementedException();
        }
        public void Write()
        {
            using (var indexFileWriter = File.Open(this.indexFile, FileMode.OpenOrCreate, FileAccess.Write))
            {
                foreach (var item in this.fileBlocks)
                {
                    var dataBytes = item.ToHeaderBytes();
                    var header = BitConverter.GetBytes(dataBytes.Length);
                    indexFileWriter.Write(header, 0, header.Length);
                    indexFileWriter.Write(dataBytes, 0, dataBytes.Length);
                }
                indexFileWriter.Flush();
            }
        }

        public void Recovery()
        {
            foreach (var item in this.fileBlocks)
            {
                var offset = item.Offset;
                var fileName = item.FileName;
                var fileInfo = new FileInfo(Directory.GetCurrentDirectory() + "\\data\\" + fileName);
                if (!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }
                dataFileStream.Seek(offset * 8, SeekOrigin.Begin);
                Block block = Block.ReadBlock(dataFileStream);
                File.WriteAllBytes(fileInfo.FullName, Decompress(block.Data));
            }
        }
        byte[] GetCompressedData(Stream stream)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                using (GZipStream zipStream = new GZipStream(outStream, CompressionMode.Compress, true))
                {
                    stream.CopyTo(zipStream);
                }
                return outStream.ToArray();
            }
        }
        byte[] Decompress(byte[] inputBytes)
        {
            using (MemoryStream originalFileStream = new MemoryStream(inputBytes))
            {

                using (MemoryStream decompressedFileStream = new MemoryStream())
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                    }
                    return decompressedFileStream.ToArray();
                }
            }
        }
        byte[] sha(byte[] data)
        {
            using (var sha = SHA1.Create())
            {
                return sha.ComputeHash(data);
            }
        }
    }
}