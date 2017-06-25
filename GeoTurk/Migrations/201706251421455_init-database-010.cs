namespace GeoTurk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initdatabase010 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkerHITs", "Status", c => c.Int(nullable: false));
            DropColumn("dbo.WorkerHITs", "IsPaid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WorkerHITs", "IsPaid", c => c.Boolean(nullable: false));
            DropColumn("dbo.WorkerHITs", "Status");
        }
    }
}
