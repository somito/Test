namespace Test.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Plays",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        GameID = c.Int(nullable: false),
                        PlayNo = c.String(),
                        PlayCoordinates = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Games", t => t.GameID, cascadeDelete: true)
                .Index(t => t.GameID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Plays", "GameID", "dbo.Games");
            DropIndex("dbo.Plays", new[] { "GameID" });
            DropTable("dbo.Plays");
        }
    }
}
