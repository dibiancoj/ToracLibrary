namespace ToracLibrary.UnitTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddT4TemplateTestTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.T4TemplateTest",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: false),
                        Description = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);


            Sql("Insert Into dbo.T4TemplateTest (Id,Description) Values(1, 'Item1')");
            Sql("Insert Into dbo.T4TemplateTest (Id,Description) Values(2, 'Item2')");
            Sql("Insert Into dbo.T4TemplateTest (Id,Description) Values(3, 'Item3')");
        }
        
        public override void Down()
        {
            DropTable("dbo.T4TemplateTest");
        }

    }
}
