namespace ToracLibraryTest.UnitsTest.EntityFramework.DataContext
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class EntityFrameworkEntityDP : DbContext
    {
        public EntityFrameworkEntityDP()
            : base("name=EntityFrameworkEntityDP")
        {
        }

        public virtual DbSet<Ref_Test> Ref_Test { get; set; }

        public virtual DbSet<Animal> Animals { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ref_Test>()
                .Property(e => e.Description)
                .IsUnicode(false);

            //data inheritance examples 
            //Animal is the abstract class
            //Cat and Dog are the derived classes
            modelBuilder.Entity<Animal>().ToTable("Animal");
            modelBuilder.Entity<Cat>().ToTable("Cat");
            modelBuilder.Entity<Dog>().ToTable("Dog");

        }
    }
}
