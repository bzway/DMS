using Bzway.Data.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppTest
{
    public class Program
    {
        public static void Main(string[] args)
        {

            using (var db = OpenDatabase.GetDatabase())
            {
                //test add 
                db.Entity<UserForTest>().Insert(new UserForTest() { Name = "test", Sex = 0 });
                db.Entity<UserForTest>().Insert(new UserForTest() { Name = "Adm Zhu", Sex = 1 });
                var result = db.Entity<UserForTest>().Query("Name", "Sex").Count();
                Console.WriteLine(result);

               
                //test delete
                //test update

                //test query
                Console.ReadKey();
            }



        }
        public class UserForTest : EntityBase
        {
            public string Name { get; set; }
            public int Sex { get; set; }
        }

    }
}
