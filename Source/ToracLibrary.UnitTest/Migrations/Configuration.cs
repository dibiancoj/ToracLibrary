namespace ToracLibrary.UnitTest.Migrations
{
    using EntityFramework.DataContext;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EntityFrameworkEntityDP>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;

            //if you don't want the code to automatically update the database then uncomment this.
            //if you uncomment this then you will have to do the update-database to update the database - instead of having the code automatically do this
            //Database.SetInitializer<EntityFrameworkEntityDP>(null);
        }

        protected override void Seed(EntityFrameworkEntityDP context)
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
        }
    }
}
