namespace GeoTurk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initdatabase013 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HITs", "CreateDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HITs", "CreateDate");
        }
    }
}
