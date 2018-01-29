namespace Test.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class namechange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Plays", "Coordinates", c => c.String());
            DropColumn("dbo.Plays", "PlayCoordinates");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Plays", "PlayCoordinates", c => c.String());
            DropColumn("dbo.Plays", "Coordinates");
        }
    }
}
