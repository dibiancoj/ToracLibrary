namespace ToracLibraryTest.UnitsTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Animal",
                c => new
                    {
                        AnimalId = c.Int(nullable: false, identity: true),
                        Size = c.String(),
                    })
                .PrimaryKey(t => t.AnimalId);
            
            CreateTable(
                "dbo.Ref_Test",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Cat",
                c => new
                    {
                        AnimalId = c.Int(nullable: false),
                        Meow = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AnimalId)
                .ForeignKey("dbo.Animal", t => t.AnimalId)
                .Index(t => t.AnimalId);
            
            CreateTable(
                "dbo.Dog",
                c => new
                    {
                        AnimalId = c.Int(nullable: false),
                        Bark = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AnimalId)
                .ForeignKey("dbo.Animal", t => t.AnimalId)
                .Index(t => t.AnimalId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Dog", "AnimalId", "dbo.Animal");
            DropForeignKey("dbo.Cat", "AnimalId", "dbo.Animal");
            DropIndex("dbo.Dog", new[] { "AnimalId" });
            DropIndex("dbo.Cat", new[] { "AnimalId" });
            DropTable("dbo.Dog");
            DropTable("dbo.Cat");
            DropTable("dbo.Ref_Test");
            DropTable("dbo.Animal");
        }
    }
}
