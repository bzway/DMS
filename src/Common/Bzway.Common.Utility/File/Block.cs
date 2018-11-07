using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.IO.Compression;

namespace Bzway.Common.Utility
{

    public class Block
    {
        /// <summary>
        /// 1 byte: 0 delete
        /// </summary>
        public byte Flag { get; set; }
        /// <summary>
        /// 4 bytes: time to life
        /// </summary>
        public uint TTL { get; set; }

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
        public string Hash { get; set; }
        /// <summary>
        /// maximum 256 characters
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 4 bytes: password to get data
        /// </summary>
        public uint SubKey { get; set; }
        /// <summary>
        /// maximum 256 characters
        /// </summary>
        public string Mime { get; set; }
        public byte[] Data { get; private set; }
        public Block(uint offSet, string fileName, string hash, string mime, uint subKey, byte[] data, uint cookie = 0, uint ttl = 0, byte flag = 1)
        {
            if (mime.Length > 256)
            {
                mime = mime.Remove(256);
            }
            this.Mime = mime;
            this.SubKey = subKey;
            this.Data = data;

            this.Offset = offSet;
            this.FileName = fileName;
            this.Hash = hash;
            this.TTL = ttl;
            this.Flag = flag;
            if (cookie == 0)
            {
                byte[] buffer = ValidateCodeGenerator.NextBytes(4);
                this.Cookie = BitConverter.ToUInt32(buffer, 0);
            }
            else
            {
                this.Cookie = cookie;
            }

        }

        public static Block ReadBlock(Stream stream)
        {
            byte[] buffer = new byte[33];
            stream.Read(buffer, 0, 33);
            var flag = buffer[0];
            var ttl = BitConverter.ToUInt32(buffer, 1);
            var subKey = BitConverter.ToUInt32(buffer, 5);
            var cookie = BitConverter.ToUInt32(buffer, 9);
            var hash = buffer.Skip(13).Take(20).ToArray();

            var size = stream.ReadByte();
            var fileName = new byte[size];
            stream.Read(fileName, 0, size);
            size = stream.ReadByte();
            var mime = new byte[size];
            stream.Read(mime, 0, size);

            buffer = new byte[4];
            stream.Read(buffer, 0, 4);
            var dataSize = BitConverter.ToUInt32(buffer, 0);
            //todo 如果文件特别大，需要特别处理
            var data = new byte[dataSize];
            stream.Read(data, 0, (int)dataSize);

            return new Block(0, Encoding.UTF8.GetString(fileName), Convert.ToBase64String(hash), Encoding.UTF8.GetString(mime), subKey, data, cookie, ttl, flag);
        }
        public byte[] ToBytes()
        {
            List<byte> list = new List<byte>();
            list.Add(this.Flag);
            list.AddRange(BitConverter.GetBytes(this.TTL));
            list.AddRange(BitConverter.GetBytes(this.SubKey));
            list.AddRange(BitConverter.GetBytes(this.Cookie));
            list.AddRange(Encoding.ASCII.GetBytes(this.Hash));
            var fileName = Encoding.UTF8.GetBytes(this.FileName);
            var size = (byte)fileName.Length;
            list.Add(size);
            list.AddRange(fileName);

            var mime = Encoding.UTF8.GetBytes(this.Mime);
            size = (byte)mime.Length;
            list.Add(size);
            list.AddRange(mime);
            uint dataSize = (uint)this.Data.LongLength;
            list.AddRange(BitConverter.GetBytes(dataSize));
            list.AddRange(this.Data);
            var padding = list.Count % 8;
            if (padding != 0)
            {
                list.AddRange(new byte[8 - padding]);
            }
            return list.ToArray();
        }
    }

}