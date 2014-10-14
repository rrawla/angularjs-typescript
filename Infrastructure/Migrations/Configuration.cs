namespace Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Infrastructure.Data;

    internal sealed class Configuration : DbMigrationsConfiguration<Infrastructure.Data.AppContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Infrastructure.Data.AppContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //


            context.Settings.AddOrUpdate(
              s => s.Name,
              new Setting { Name = "Email Server" , Value = "smtp.foo.com" },
              new Setting { Name = "Email Server Port", Value = "25" },
              new Setting { Name = "Email Server User" , Value = "emailservice@foo.com" },
              new Setting { Name = "Email Server Pass" , Value = "1234" },
              new Setting { Name = "Email From" , Value = "donotreply@foo.com" }
            );
        }
    }
}
