using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Bzway.Sites.SmartBackend.Models;
using Bzway.Framework.Application;
using Microsoft.Extensions.Logging;

namespace Bzway.Sites.SmartBackend.Controllers
{
    public class ProductController : ApiController<Product, ProductController>
    {
        #region ctor
        readonly IUserService userService;
        public ProductController(
            IUserService userService,
            ILoggerFactory loggerFactory,
            ITenant tenant) : base(loggerFactory, tenant)
        {
            this.userService = userService;
        }
        #endregion
        public IActionResult Index(string categroy, string tags, int? pageIndex, int? pageSize)
        {
            var query = this.db.Query();
            if (!string.IsNullOrEmpty(categroy))
            {
                query = query.Where(m => m.CategoryId == categroy);
            }
            if (!string.IsNullOrEmpty(tags))
            {
                query = query.Where(m => m.ProductTags.Contains(tags));
            }
            var list = query.Skip((pageIndex ?? 1 - 1) * (pageSize ?? 10)).Take(pageSize ?? 10).ToList();
            return View(list);
        }
        public IActionResult Detail(string id)
        {
            var item = this.db.Query().FirstOrDefault(m => m.Id == id);
            ProductViewModel model = new ProductViewModel()
            {
                CategoryId = item.CategoryId,
                CommentList = this.repository.Entity<ProductComment>().Query().Where(m => m.ProductId == item.Id).ToList(),
                CreatedBy = item.CreatedBy,
                CreatedOn = item.CreatedOn,
                Description = item.Description,
                Id = item.Id,
                Image = item.Image,
                ImageList = this.repository.Entity<ProductImage>().Query().Where(m => m.ProductId == item.Id).ToList(),
                Name = item.Name,
                Price = item.Price,
                ProductTags = item.ProductTags,
                Remark = item.Remark,
                UpdatedBy = item.UpdatedBy,
                UpdatedOn = item.UpdatedOn,
                Url = item.Url,
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            UserManager userManager = AppEngine.GetService<UserManager>();
            var status = userManager.PasswordSignIn(model.UserName, model.Password, true);
            ModelState.AddModelError("", "LoginModelRequired");
            return View();
        }
    }
}