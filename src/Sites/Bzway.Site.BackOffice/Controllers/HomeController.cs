using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bzway.Module.Wechat.Interface;
using Microsoft.Extensions.Logging;
using System.Threading;
using Bzway.Framework.Application;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Bzway.Common.Share.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Bzway.Sites.BackOffice.Controllers
{

    public class HomeController : BaseController<HomeController>
    {
        #region ctor
        public HomeController(ITenant tenant, ILoggerFactory loggerFactory) : base(loggerFactory, tenant) { }
        #endregion

        static IQuickSearch<MobileModel> search;

        [Authorize(Roles = "Admin")]
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Mobile()
        {
            List<MobileModel> list = new List<MobileModel>();
            if (search == null)
            {
                foreach (var item in System.IO.File.ReadAllLines("d:\\mobile.txt"))
                {
                    //1,1300000,山东,济南,中国联通,0531,250000
                    var data = item.Split(',');
                    list.Add(new MobileModel()
                    {
                        Prefix = int.Parse(data[1]),
                        Proince = data[2],
                        City = data[3],
                        Vendor = data[4],
                        AreaCode = data[5],
                        PostCode = data[6],
                    });
                }

                search = QuickSearch.BuildQuickSearch<MobileModel>(list.ToArray());
            }
            list = new List<MobileModel>();
            foreach (var item in "".Split(','))
            {
                var x = search.Search(new MobileModel() { Prefix = int.Parse(item.Substring(0, 7)) });
                if (x != null)
                {
                    list.Add(x);
                }
            }
            return View(list);
        }

        [Serializable]
        public class MobileModel : IComparable<MobileModel>
        {
            public string Proince { get; set; }
            public string City { get; set; }
            public string Vendor { get; set; }
            public string PostCode { get; set; }
            public string AreaCode { get; set; }

            public int Prefix { get; set; }

            public int CompareTo(MobileModel other)
            {
                return this.Prefix.CompareTo(other.Prefix);
            }

            public override int GetHashCode()
            {
                return Prefix;
            }
        }
    }
}