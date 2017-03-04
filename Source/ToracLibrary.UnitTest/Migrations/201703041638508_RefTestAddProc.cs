namespace ToracLibrary.UnitTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RefTestAddProc : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE PROCEDURE [dbo].[RefTestStoredProcTest]
                    @Id Int
                AS
                BEGIN

                SET NOCOUNT ON;
        
                    SELECT Id
                    FROM Ref_Test AS T
                    WHERE T.Id = @Id;

                END");
        }
        
        public override void Down()
        {
            Sql("DROP PROCEDURE [dbo].[RefTestStoredProcTest]");
        }
    }
}
