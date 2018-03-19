namespace Test.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScorerGolie : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Plays", "ScorerID", c => c.String());
            AddColumn("dbo.Plays", "GolieID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Plays", "GolieID");
            DropColumn("dbo.Plays", "ScorerID");
        }
    }
}
