namespace GeoTurk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initdatabase005 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HITs", "WorkersCount", c => c.Int(nullable: false, defaultValue: 1));
            AddColumn("dbo.HITs", "Cost", c => c.Decimal(nullable: false, precision: 10, scale: 2, defaultValue: 0));
            AddColumn("dbo.Users", "Balance", c => c.Decimal(nullable: false, precision: 10, scale: 2, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Balance");
            DropColumn("dbo.HITs", "Cost");
            DropColumn("dbo.HITs", "WorkersCount");
        }
    }
}
