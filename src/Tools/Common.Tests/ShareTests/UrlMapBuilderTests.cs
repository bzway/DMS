using Bzway.Common.Script;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.Tests
{
    [TestClass]
    public class UrlMapBuilderTests
    {

        [TestMethod]
        public void UrlMapTest1()
        {
            var build = new UrlMapBuilder()
               //.Map("/$Action")
               //.Map("/$Controller")
               //.Map("/$Controller/$Action/")
               .Map("/$Controller=Home/$Action=Index/$Id:int")
               .Build();
            var action = build.Action("home/index/12", "D:\\electron\\");
            var content = action.Render().Result;

            System.Diagnostics.Debug.WriteLine(content);
        }
    }
}
