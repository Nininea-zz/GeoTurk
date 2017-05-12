namespace GeoTurk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initdatabase004 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkerHITs",
                c => new
                    {
                        WorkerID = c.Int(nullable: false),
                        HITID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.WorkerID, t.HITID })
                .ForeignKey("dbo.HITs", t => t.HITID)
                .ForeignKey("dbo.Users", t => t.WorkerID)
                .Index(t => t.WorkerID)
                .Index(t => t.HITID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkerHITs", "WorkerID", "dbo.Users");
            DropForeignKey("dbo.WorkerHITs", "HITID", "dbo.HITs");
            DropIndex("dbo.WorkerHITs", new[] { "HITID" });
            DropIndex("dbo.WorkerHITs", new[] { "WorkerID" });
            DropTable("dbo.WorkerHITs");
        }
    }
}
