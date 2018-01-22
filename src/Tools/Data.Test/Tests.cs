using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class Tests
    {
        public void TestDateTime()
        {
            DateTime baseTime = new DateTime(1970, 1, 1);
            var expected = (long)(DateTime.UtcNow - baseTime).TotalSeconds;
            var v1 = DateTime.Now.ToFileTimeUtc();
            var v2 = DateTime.UtcNow.ToFileTime();
            Assert.Equals(expected, v1);
            Assert.Equals(expected, v2);
        }

        [TestMethod]
        public void Test1()
        {
            //using (var db = OpenDatabase.GetDatabase("SQLServer"))
            //{
            //    db.Entity<test>().Insert(new test() { Name = "test", UUID = "test" });
            //    var entity = db.Entity<test>().Query().Where(m => m.UUID, "test", CompareType.Equal).First();
            //    Assert.IsNull(entity);
            //    db.Entity<test>().Update(new test() { Name = "updated", UUID = "test" });
            //    entity = db.Entity<test>().Query().Where(m => m.Name, "updated", CompareType.Equal).First();
            //    Assert.IsNotNull(entity);
            //    db.Entity<test>().Delete("test");
            //    entity = db.Entity<test>().Query().Where(m => m.Name, "updated", CompareType.Equal).First();
            //    Assert.IsNull(entity);
            //    var list = db.Entity<test>().Query().ToList();
            //}
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestGenerate()
        {
            Assert.IsTrue(true);
        }
    }
}