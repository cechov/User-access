using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Brukertilgang.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Display(Name = "RoleName")]
        public string Name { get; set; }
    }

    public class UserViewModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}