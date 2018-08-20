using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using Bzway.Common.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class Tests
    {

        [TestMethod]
        public void Test1()
        {
            Parallel.ForEach("1,2".Split(','), i =>
            {
                using (SqlConnection connection = new SqlConnection("data source=172.31.238.2;user ID=devhaagen;password=qawsed1!;persist security info=True;initial catalog=DHAAGEN_DAZS_CHINA_CLUB;Connection Timeout=24000;"))
                {
                    connection.Open();
                    var cmd = connection.CreateCommand();
                    cmd.CommandText = @"UPDATE TOP (2066400)  coupon SET tag = @tag_txt WHERE tag = ''".Replace("@tag_txt", i);
                    var count = cmd.ExecuteNonQuery();
                    Console.WriteLine(string.Format("{0}:{1}", i, count));
                }

            });
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestSimpleWebToken()
        {
            var payload = new { i = "", n = "", e = DateTime.Now.ToString() };
            var token = string.Empty;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i < 10; i++)
            {
                token = SimpleWebToken.Encode(payload, "MD5");
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(SimpleWebToken.Decode(token)));
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedTicks);
        }
    }
}