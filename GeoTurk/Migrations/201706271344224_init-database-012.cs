namespace GeoTurk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initdatabase012 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HITs", "IsCompleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.HITs", "IsMaxedOut", c => c.Boolean(nullable: false));
            AddColumn("dbo.TransactionLogs", "CreateDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TransactionLogs", "CreateDate");
            DropColumn("dbo.HITs", "IsMaxedOut");
            DropColumn("dbo.HITs", "IsCompleted");
        }
    }
}
