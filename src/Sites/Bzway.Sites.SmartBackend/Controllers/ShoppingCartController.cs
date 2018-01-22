using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bzway.Sites.SmartBackend.Models;
using Microsoft.Extensions.Logging;
using Bzway.Framework.Application;

namespace Bzway.Sites.SmartBackend.Controllers
{
    public class ShoppingCartResult
    {
        public int TotalItem { get; set; }
        public int ProductItem { get; set; }

    }
    public class ShoppingCartController : ApiController<ShoppingCart, ShoppingCartController>
    {
        public ShoppingCartController(ILoggerFactory loggerFactory,
            ITenant tenant) : base(loggerFactory, tenant)
        { }

        public IActionResult Index()
        {
            List<ShoppingCartViewModel> list = new List<ShoppingCartViewModel>();
            foreach (var item in this.db.Query().Where(m => m.UserId == this.User.Identity.ToString()))
            {
                var product = this.repository.Entity<Product>().Query().Where(m => m.Id == item.ProductId).FirstOrDefault();
                if (product == null)
                {
                    continue;
                }
                list.Add(new ShoppingCartViewModel()
                {
                    ProductName = product.Name,
                    ProductId = product.Id,
                    ProductImage = product.Image,
                    Number = item.Number,
                    ProductPrice = product.Price,
                    Remark = item.CreatedOn.ToString("yyyy-MM-dd hh:mm:ss")
                });

            }
            return View(list);
        }
        public IActionResult Address()
        {

            return View();
        }
        [HttpPost]
        public Result<ShoppingCartResult> AddItem(string Id, int? number)
        {
            var userId = this.User.Identity.ToString();
            if (string.IsNullOrEmpty(userId))
            {
                return new Result<ShoppingCartResult>()
                {
                    Code = -1,
                    ErrorMessage = "请重新登录",
                    Data = new ShoppingCartResult() { ProductItem = 0, TotalItem = 0 },
                };
            }

            var item = this.db.Query().Where(m => m.ProductId == Id && m.UserId == userId).FirstOrDefault();
            if (item == null)
            {
                item = new ShoppingCart()
                {
                    ProductId = Id,
                    Number = number ?? 1,
                    UserId = userId,
                    Id = Guid.NewGuid().ToString(),
                    CreatedBy = string.Empty,
                    CreatedOn = DateTime.Now,
                    UpdatedBy = string.Empty,
                    UpdatedOn = DateTime.Now,
                };
                this.db.Insert(item);

            }
            else
            {
                item.Number += number ?? 1;
                this.db.Update(item);
            }
            var list = this.db.Query().Where(m => m.UserId == userId).ToList();

            return new Result<ShoppingCartResult>()
            {
                Code = 0,
                ErrorMessage = string.Empty,
                Data = new ShoppingCartResult() { ProductItem = list.Where(m => m.ProductId == Id).First().Number, TotalItem = list.Sum(m => m.Number) },
            };
        }

        [HttpPost]
        public Result<ShoppingCartResult> RemoveItem(string Id, int? number)
        {
            var userId = this.User.Identity.ToString();
            if (string.IsNullOrEmpty(userId))
            {
                return new Result<ShoppingCartResult>()
                {
                    Code = -1,
                    ErrorMessage = "请重新登录",
                    Data = new ShoppingCartResult() { ProductItem = 0, TotalItem = 0 },
                };
            }

            var item = this.db.Query().Where(m => m.ProductId == Id && m.UserId == userId).FirstOrDefault();
            if (item == null)
            {
                return new Result<ShoppingCartResult>()
                {
                    Code = 0,
                    ErrorMessage = string.Empty,
                    Data = new ShoppingCartResult() { ProductItem = 0, TotalItem = 0 },
                };
            }
            if (number.HasValue && item.Number > number.Value)
            {
                item.Number -= number.Value;
                this.db.Update(item);
            }
            else
            {
                this.db.Delete(item);
            }
            var list = this.db.Query().Where(m => m.UserId == userId).ToList();
            return new Result<ShoppingCartResult>()
            {
                Code = 0,
                ErrorMessage = string.Empty,
                Data = new ShoppingCartResult() { ProductItem = list.Where(m => m.ProductId == Id).Sum(m => m.Number), TotalItem = list.Sum(m => m.Number) },
            };
        }

        [HttpPost]
        public string Clear()
        {
            var userId = this.User.Identity.ToString();
            if (string.IsNullOrEmpty(userId))
            {
                return "请重新登录";
            }
            foreach (var item in db.Query().Where(m => m.UserId == userId).ToList())
            {
                this.db.Delete(item.Id);
            }
            return string.Empty;
        }


        [HttpPost]
        public Result<int> Product(string pid)
        {
            var userId = this.User.Identity.ToString();

            if (string.IsNullOrEmpty(pid))
            {
                return Result.OK<int>(this.db.Query().Where(m => m.UserId == userId).Sum(m => m.Number));
            }

            var item = this.db.Query().Where(m => m.ProductId == pid && m.UserId == userId).FirstOrDefault();
            if (item == null)
            {
                return Result.OK<int>(0);
            }
            else
            {
                return Result.OK<int>(item.Number);
            }
        }
    }
}
