using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Bzway.Sites.SmartBackend.Models
{

    public class ShoppingCartViewModel
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Number { get; set; }
        public string ProductImage { get; set; }
        public decimal ProductPrice { get; set; }
        public string Remark { get; set; }
    }


    public class ProductViewModel : Product
    {
        public IList<ProductImage> ImageList { get; set; }
        public IList<ProductComment> CommentList { get; set; }
    }


}