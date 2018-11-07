using Bzway.Common.Script;
using Bzway.Common.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common.Script.Tests
{
    [TestClass]
    public class FileTests
    {

        [TestMethod]
        public void FileConvertersTest1()
        {
            var data = Encoding.UTF8.GetBytes("testtesttesttesttest");
            BlockIndex index1 = new BlockIndex(0, "test", "testtesttesttesttest", 3, 2, 1);
            var index2 = BlockIndex.ReadBlockIndex(index1.ToHeaderBytes());
            Block block1 = new Block(index1.Offset, index1.FileName, index1.Hash, "mime", 0, data, index1.Cookie, index1.TTL, index1.Flag);

            Block block2 = Block.ReadBlock(new MemoryStream(block1.ToBytes()));
            var data1 = Encoding.UTF8.GetString(block1.Data);
            var data2 = Encoding.UTF8.GetString(block2.Data);
            Assert.IsTrue(string.Equals(data1, data2), "数据不一致");
        }
        [TestMethod]
        public void FileWriteAndReadTest1()
        {
            FileManager manager = new FileManager();
            foreach (var item in Directory.GetFiles("D:\\Work\\go", "*.*", SearchOption.AllDirectories))
            {
                FileInfo info = new FileInfo(item);
                using (var stream = info.OpenRead())
                {
                    var path = string.Join('/', info.DirectoryName.Split('\\', '/').Skip(1));
                    manager.AddFile(info.Name, info.Extension, stream, stream.Length, path);
                }
            }

        }

        [TestMethod]
        public void FileWriteTest1()
        {
            FileManager manager = new FileManager();

            manager.Recovery();
        }


    }
}
