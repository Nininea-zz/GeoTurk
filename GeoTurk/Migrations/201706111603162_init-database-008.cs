namespace GeoTurk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initdatabase008 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkerHITs", "CompleteDate", c => c.DateTime());
            AddColumn("dbo.WorkerHITs", "IsPaid", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkerHITs", "IsPaid");
            DropColumn("dbo.WorkerHITs", "CompleteDate");
        }
    }
}
