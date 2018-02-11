using Brukertilgang.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Brukertilgang.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        
        public ActionResult Index()
        {
            var usersWithRoles = (from user in context.Users
                                  from userRole in user.Roles
                                  join role in context.Roles on userRole.RoleId equals
                                  role.Id
                                  select new UserViewModel()
                                  {
                                      UserId = user.Id,
                                      Username = user.UserName,
                                      Role = role.Name
                                  }).ToList();

            return View(usersWithRoles);
        }

        // GET: /Admin/Edit/5
        public ActionResult Edit(string id)
        {
            var store = new UserStore<ApplicationUser>(context);
            var manager = new UserManager<ApplicationUser>(store);
            var rstore = new RoleStore<IdentityRole>(context);
            var rmanager = new RoleManager<IdentityRole>(rstore);

            if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
            
            var user = context.Users.Find(id);
            var userRoles = manager.GetRoles(user.Id);

            if (user != null)
            {
                UserViewModel x = new UserViewModel() {
                    UserId = user.Id,
                    Username = user.UserName,
                    RolesList = rmanager.Roles.ToList().Select(rr => new SelectListItem()
                    {
                        Selected = userRoles.Contains(rr.Name),
                        Text = rr.Name,
                        Value = rr.Name
                    })
                };
                return View(x);
            }
            else { return HttpNotFound(); }
        }

        // POST: /Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            var store = new UserStore<ApplicationUser>(context);
            var manager = new UserManager<ApplicationUser>(store);
            var rstore = new RoleStore<IdentityRole>(context);
            var rmanager = new RoleManager<IdentityRole>(rstore);
            var user = context.Users.Find(id);

            try
            {
                if (user == null)
                {
                    return HttpNotFound();
                }
                string newRole = collection["Role"].ToString();
                string[] oldRole = manager.GetRoles(user.Id).ToArray(); 
                manager.RemoveFromRoles(user.Id, oldRole);
                manager.AddToRoles(user.Id, newRole);
                return RedirectToAction("Index");
            }
            catch
            {
                var userRoles = manager.GetRoles(user.Id).ToArray();
                UserViewModel x = new UserViewModel()
                {
                    UserId = user.Id,
                    Username = user.UserName,
                    RolesList = rmanager.Roles.ToList().Select(rr => new SelectListItem()
                    {
                        Selected = userRoles.Contains(rr.Name),
                        Text = rr.Name,
                        Value = rr.Name
                    })
                };
                return View(x);
            }
        }
    }
}
