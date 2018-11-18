namespace TopStocks.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using TopStocks.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TopStocks.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TopStocks.Models.ApplicationDbContext context)
        {
            // add application roles
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            if (!RoleManager.RoleExists("Admin"))
            {
                var roleResult = RoleManager.Create(new IdentityRole("Admin"));
            }
            if (!RoleManager.RoleExists("User"))
            {
                var roleResult = RoleManager.Create(new IdentityRole("User"));
            }
        }
    }
}
