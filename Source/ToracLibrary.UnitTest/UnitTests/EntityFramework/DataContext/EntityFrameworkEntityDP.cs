namespace ToracLibrary.UnitTest.EntityFramework.DataContext
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class EntityFrameworkEntityDP : DbContext
    {
        public EntityFrameworkEntityDP()
            : base("EntityFrameworkEntityDP")
        {
        }

        public virtual DbSet<Ref_Test> Ref_Test { get; set; }

        public virtual DbSet<Animal> Animals { get; set; }

        public virtual DbSet<Ref_SqlCacheTrigger> Ref_SqlCacheTriggers { get; set; }

        public virtual DbSet<T4TemplateTest> T4TemplateTests { get; set; }

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

            modelBuilder.Entity<Ref_SqlCacheTrigger>().ToTable("Ref_SqlCacheTrigger");

            modelBuilder.Entity<T4TemplateTest>().ToTable("T4TemplateTest");

        }
    }
}
