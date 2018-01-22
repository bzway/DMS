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
    public class Category : EntityBase
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string Remark { get; set; }
    }

    public class Product : EntityBase
    {
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Remark { get; set; }
        public string ProductTags { get; set; }
    }

    public class ProductImage : EntityBase
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
    public class ProductComment : EntityBase
    {
        public string ProductId { get; set; }
        public string Title { get; set; }

        public string Content { get; set; }
        public string User { get; set; }
        public int Rate { get; set; }
    }

    public class ShoppingCart : EntityBase
    {
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public int Number { get; set; }
    }

    public class Order : EntityBase
    {
        public decimal TotalPrice { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string DeliveryId { get; set; }
    }

    public class Delivery : EntityBase
    {
        public string UserId { get; set; }
        public string Province { get; set; }
        public string City { get; set; }

        public string Streat { get; set; }

        public string PostCode { get; set; }

        public string ContactName { get; set; }

        public string ContactPhone { get; set; }
    }

    public class OrderDetail : EntityBase
    {
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public int Number { get; set; }
        public string ProductName { get; set; }
        public string OriginalPrice { get; set; }
        public decimal Price { get; set; }
    }
}
