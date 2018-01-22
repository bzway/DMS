using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Bzway.Sites.SmartBackend.Models
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class GrantViewModel
    {
        public string appid { get; set; }
        public string secret { get; set; }
    }
}
