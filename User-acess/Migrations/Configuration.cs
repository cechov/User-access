namespace Brukertilgang.Migrations
{
    using Brukertilgang.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Brukertilgang.Models.ApplicationDbContext>
    {
        private const string AdministratorRole = "admin";
        private const string UserRole = "user";
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Brukertilgang.Models.ApplicationDbContext";
        }

        protected override void Seed(Brukertilgang.Models.ApplicationDbContext context)
        {

            if (!context.Roles.Any(r => r.Name == AdministratorRole))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                manager.Create(new IdentityRole { Name = AdministratorRole });
            }

            if (!context.Roles.Any(r => r.Name == UserRole))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                manager.Create(new IdentityRole { Name = UserRole });
            }

            CreateOrUpdateUser(context, "defaultadmin");
        }

        private static void CreateOrUpdateUser(ApplicationDbContext context, string username)
        {
            if (!context.Users.Any(u => u.UserName == username))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = username + "@gmail.com", Email = username + "@gmail.com" };

                manager.Create(user, "access2WEB!");

                manager.AddToRole(user.Id, AdministratorRole);
            }
        }
    }
}
