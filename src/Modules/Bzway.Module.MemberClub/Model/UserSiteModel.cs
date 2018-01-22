using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
namespace Bzway.Module.MemberClub
{
    public class UserSiteModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone number")]
        public string SiteName { get; set; }

        public List<string> Host { get; set; }
    }
}
