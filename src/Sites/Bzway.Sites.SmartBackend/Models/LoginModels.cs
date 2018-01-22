using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Bzway.Sites.SmartBackend.Models
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class LoginModel
    {
        [Required(ErrorMessage = "用户名不能为空")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }

    }
}