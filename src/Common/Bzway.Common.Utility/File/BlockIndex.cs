using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.IO.Compression;

namespace Bzway.Common.Utility
{
    public class BlockIndex
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
        public BlockIndex(uint offSet, string fileName, string hash, uint cookie = 0, uint ttl = 0, byte flag = 1)
        {
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
        public static BlockIndex ReadBlockIndex(byte[] header)
        {
            var flag = header[0];
            var ttl = BitConverter.ToUInt32(header, 1);
            var offset = BitConverter.ToUInt32(header, 5);
            var cookie = BitConverter.ToUInt32(header, 9);
            var hash = header.Skip(13).Take(20).ToArray();
            var size = header[33];
            var fileName = header.Skip(34).Take(size).ToArray();
            return new BlockIndex(offset, Encoding.UTF8.GetString(fileName), Convert.ToBase64String(hash, 0, hash.Length), cookie, ttl, flag);
        }
        public byte[] ToHeaderBytes()
        {
            List<byte> list = new List<byte>();
            list.Add(this.Flag);
            list.AddRange(BitConverter.GetBytes(this.TTL));
            list.AddRange(BitConverter.GetBytes(this.Offset));
            list.AddRange(BitConverter.GetBytes(this.Cookie));
            list.AddRange(Encoding.ASCII.GetBytes(this.Hash));
            var fileNameData = Encoding.UTF8.GetBytes(this.FileName).Take(256).ToArray();
            list.Add((byte)fileNameData.Length);
            list.AddRange(fileNameData);
            return list.ToArray();
        }
    }
}