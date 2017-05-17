namespace GeoTurk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initdatabase007 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HITs", "PublishDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HITs", "PublishDate");
        }
    }
}
