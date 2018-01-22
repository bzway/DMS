using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Bzway.Database.Core;

namespace Bzway.Sites.SmartBackend.Models
{
    public class Store : EntityBase
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string Remark { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Distinct { get; set; }
        public string Street { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public decimal Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public decimal Latitude { get; set; }
    }

    public class ProductStore : EntityBase
    {
        public string PorductId { get; set; }
        public string StoreId { get; set; }
    }
}
