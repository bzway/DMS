using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace WindowsExplorer
{
    public class ByteComparer : IEqualityComparer<byte[]>
    {
        public bool Equals(byte[] a1, byte[] a2)
        {
            return a1.SequenceEqual(a2);
        }


        public int GetHashCode(byte[] obj)
        {
            if (obj == null || obj.Length < 4)
            {
                return 0;
            }
            return BitConverter.ToInt32(obj, 0);
        }
    }
    public static class Program
    {
        public static string GetString(this byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {



            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form3());
        }
    }
    public class BlockIndex
    {
        /// <summary>
        /// 1 byte: 0 delete
        /// </summary>
        protected byte Flag { get; set; }
        /// <summary>
        /// 4 bytes: time to life
        /// </summary>
        protected uint TTL { get; set; }

        /// <summary>
        /// 4 bytes: block offset (assigned to 8) in superblock
        /// </summary>
        public uint Offset { get; set; }
        /// <summary>
        /// 4 bytes: random number to mitigate brute force lookups
        /// </summary>
        public uint Cookie { get; set; }
        /// <summary>
        /// 20 bytes: for check sum
        /// </summary>
        public byte[] Hash { get; set; }
        /// <summary>
        /// maximum 256 characters
        /// </summary>
        public byte[] FileName { get; set; }

        private Random random = new Random();

        public BlockIndex(uint offSet, byte[] fileName, byte[] hash, uint cookie = 0, uint ttl = 0, byte flag = 1)
        {
            this.Offset = offSet;
            this.FileName = fileName;
            this.Hash = hash;
            this.TTL = ttl;
            this.Flag = flag;
            if (cookie == 0)
            {
                byte[] buffer = new byte[4];
                random.NextBytes(buffer);
                this.Cookie = BitConverter.ToUInt32(buffer, 0);
            }
            else
            {
                this.Cookie = cookie;
            }
        }
        public BlockIndex(byte[] header)
        {
            if (header == null || header.Length <= 33)
            {
                return;
            }
            this.Flag = header[0];
            this.TTL = BitConverter.ToUInt32(header, 1);
            var test = BitConverter.GetBytes((uint)2);

            this.Offset = BitConverter.ToUInt32(header, 5);
            this.Cookie = BitConverter.ToUInt32(header, 9);
            this.Hash = header.Skip(13).Take(20).ToArray();
            var size = header[33];
            this.FileName = header.Skip(34).Take(size).ToArray();
            var t = Encoding.UTF8.GetString(FileName);
        }
        public byte[] ToHeaderBytes()
        {

            List<byte> list = new List<byte>();
            list.Add(this.Flag);
            list.AddRange(BitConverter.GetBytes(this.TTL));
            list.AddRange(BitConverter.GetBytes(this.Offset));
            list.AddRange(BitConverter.GetBytes(this.Cookie));
            list.AddRange(this.Hash);
            byte size = (byte)this.FileName.Length;
            list.Add(size);
            list.AddRange(this.FileName);
            return list.ToArray();
        }
    }
    public class Block : BlockIndex
    {


        /// <summary>
        /// 4 bytes: password to get data
        /// </summary>
        public uint SubKey { get; set; }
        /// <summary>
        /// maximum 256 characters
        /// </summary>
        public byte[] Mime { get; set; }
        public byte[] Data { get; private set; }
        public Block(byte[] header, string mime, uint subKey) : base(header)
        {
            if (mime.Length > 256)
            {
                mime = mime.Remove(256);
            }
            this.Mime = Encoding.UTF8.GetBytes(mime);
            this.SubKey = subKey;

        }

        public Block(Stream stream) : base(null)
        {
            byte[] buffer = new byte[33];
            stream.Read(buffer, 0, 33);
            this.Flag = buffer[0];
            this.TTL = BitConverter.ToUInt32(buffer, 1);
            this.SubKey = BitConverter.ToUInt32(buffer, 5);
            this.Cookie = BitConverter.ToUInt32(buffer, 9);
            this.Hash = buffer.Skip(13).Take(20).ToArray();

            var size = stream.ReadByte();
            this.FileName = new byte[size];
            stream.Read(this.FileName, 0, size);
            size = stream.ReadByte();
            var test = Encoding.UTF8.GetString(this.FileName);

            this.Mime = new byte[size];
            stream.Read(this.Mime, 0, size);
            buffer = new byte[4];
            stream.Read(buffer, 0, 4);
            var dataSize = BitConverter.ToUInt32(buffer, 0);
            //todo 如果文件特别大，需要特别处理
            this.Data = new byte[dataSize];
            stream.Read(this.Data, 0, (int)dataSize);
        }
        public byte[] ToBytes(byte[] data)
        {
            List<byte> list = new List<byte>();
            list.Add(this.Flag);
            list.AddRange(BitConverter.GetBytes(this.TTL));
            list.AddRange(BitConverter.GetBytes(this.SubKey));
            list.AddRange(BitConverter.GetBytes(this.Cookie));
            list.AddRange(this.Hash);
            var size = (byte)this.FileName.Length;
            list.Add(size);
            list.AddRange(this.FileName);
            size = (byte)this.Mime.Length;
            list.Add(size);
            list.AddRange(this.Mime);
            uint dataSize = (uint)data.LongLength;
            list.AddRange(BitConverter.GetBytes(dataSize));
            list.AddRange(data);
            var padding = list.Count % 8;
            if (padding != 0)
            {
                list.AddRange(new byte[8 - padding]);
            }
            return list.ToArray();
        }
    }
    public class SuperBlock
    {
        List<BlockIndex> fileBlocks = new List<BlockIndex>();
        Dictionary<byte[], int> fileNameIndex = new Dictionary<byte[], int>(new ByteComparer());
        Dictionary<byte[], int> fileDataIndex = new Dictionary<byte[], int>(new ByteComparer());
        private string dataFile;
        private string indexFile;
        public uint VolumeId { get; set; }
        public bool ReadOnly { get; set; }
        public int TTL { get; set; }

        private static object lockObject = new object();

        private FileStream dataFileWriter;
        public SuperBlock(uint volumeId, string root)
        {
            this.VolumeId = volumeId;
            this.dataFile = Path.Combine(root, this.VolumeId + ".data");
            this.indexFile = Path.Combine(root, this.VolumeId + ".index");
            this.dataFileWriter = File.Open(this.dataFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            using (var indexFileReader = File.Open(this.indexFile, FileMode.OpenOrCreate, FileAccess.Read))
            {
                byte[] buffer = new byte[4];
                indexFileReader.Read(buffer, 0, 4);
                int size = BitConverter.ToInt32(buffer, 0);


                while (size > 0)
                {
                    buffer = new byte[size];
                    indexFileReader.Read(buffer, 0, size);
                    var block = new BlockIndex(buffer);
                    //判断文件内容是否已经存在
                    if (fileDataIndex.ContainsKey(block.Hash))
                    {
                        //判断文件名称是否已经存在
                        if (!fileNameIndex.ContainsKey(block.FileName))
                        {
                            fileNameIndex.Add(block.FileName, fileDataIndex[block.Hash]);
                        }
                        else
                        {
                            fileNameIndex[block.FileName] = fileDataIndex[block.Hash];
                        }
                        continue;
                    }
                    this.fileBlocks.Add(block);
                    var fileIndex = this.fileBlocks.Count - 1;
                    fileDataIndex.Add(block.Hash, fileIndex);
                    fileNameIndex.Add(block.FileName, fileIndex);
                    //read next one
                    buffer = new byte[4];
                    indexFileReader.Read(buffer, 0, 4);
                    size = BitConverter.ToInt32(buffer, 0);
                }
            }
        }
        public void Dispose()
        {
            this.dataFileWriter.Close();
        }
        public void AddFile(string dir, string fileName, Stream stream)
        {
            var path = string.Join("/", dir.Split('\\', '/').Skip(1).ToArray());
            //得到文件的数据
            byte[] dataBytes = GetCompressedData(stream);
            //得到文件的名称
            var filePath = Encoding.UTF8.GetBytes(path + "/" + fileName);
            //得到文件的哈希
            var hash = sha(dataBytes);
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
            var blockIndex = new BlockIndex((uint)(dataFileWriter.Position / 8), filePath, hash);
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
            Block block = new Block(blockIndex.ToHeaderBytes(), "", 0);
            dataBytes = block.ToBytes(dataBytes);
            dataFileWriter.Write(dataBytes, 0, dataBytes.Length);
            dataFileWriter.Flush();
        }
        public void AddFiles(string dir)
        {
            foreach (var item in Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories))
            {
                FileInfo info = new FileInfo(item);
                using (var stream = info.OpenRead())
                {
                    AddFile(info.DirectoryName, info.Name, stream);
                }
            }
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
                var fileName = Encoding.UTF8.GetString(item.FileName);
                var fileInfo = new FileInfo(Directory.GetCurrentDirectory() + "\\data\\" + fileName);
                if (!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }
                dataFileWriter.Seek(offset * 8, SeekOrigin.Begin);
                Block block = new Block(dataFileWriter);
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
            using (MemoryStream outStream = new MemoryStream())
            {
                using (MemoryStream inputStream = new MemoryStream())
                {
                    using (GZipStream zipStream = new GZipStream(outStream, CompressionMode.Decompress))
                    {
                        inputStream.Write(inputBytes, 0, inputBytes.Length);
                    }
                    return outStream.ToArray();
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
    public class FileManager
    {
        Dictionary<uint, SuperBlock> superBlocks = new Dictionary<uint, SuperBlock>(10);
        public FileManager()
        {
            var superBlock = new SuperBlock(1, Directory.GetCurrentDirectory() + "/dir");
            var hash = Encoding.UTF8.GetBytes("testtesttesttesttest");
            BlockIndex index = new BlockIndex(0, Encoding.UTF8.GetBytes("test"), hash, 3, 2, 1);
            var index2 = new BlockIndex(index.ToHeaderBytes());
            Block block = new Block(index.ToHeaderBytes(), "app", 4);

            Block block2 = new Block(new MemoryStream(block.ToBytes(hash)));
            this.superBlocks.Add(1, superBlock);
        }

        public void Add(string dir)
        {
            this.superBlocks[1].AddFiles(dir);
            this.superBlocks[1].Write();
            MessageBox.Show("OK");
        }
        public void Recovery()
        {
            this.superBlocks[1].Recovery();
        }
    }
}