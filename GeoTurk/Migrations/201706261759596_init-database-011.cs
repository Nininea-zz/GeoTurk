namespace GeoTurk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initdatabase011 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransactionLogs",
                c => new
                    {
                        TransactionLogID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        HITID = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.TransactionLogID)
                .ForeignKey("dbo.HITs", t => t.HITID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.HITID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TransactionLogs", "UserID", "dbo.Users");
            DropForeignKey("dbo.TransactionLogs", "HITID", "dbo.HITs");
            DropIndex("dbo.TransactionLogs", new[] { "HITID" });
            DropIndex("dbo.TransactionLogs", new[] { "UserID" });
            DropTable("dbo.TransactionLogs");
        }
    }
}
