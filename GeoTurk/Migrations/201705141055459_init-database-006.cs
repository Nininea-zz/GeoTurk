namespace GeoTurk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initdatabase006 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkerHITs", "AssignDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkerHITs", "AssignDate");
        }
    }
}
