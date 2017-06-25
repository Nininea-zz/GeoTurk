namespace GeoTurk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initdatabase009 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HITAnswers",
                c => new
                    {
                        WorkerID = c.Int(nullable: false),
                        HITID = c.Int(nullable: false),
                        TaskChoiseID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.WorkerID, t.HITID, t.TaskChoiseID })
                .ForeignKey("dbo.TaskChoises", t => t.TaskChoiseID, cascadeDelete: true)
                .ForeignKey("dbo.WorkerHITs", t => new { t.WorkerID, t.HITID }, cascadeDelete: true)
                .Index(t => new { t.WorkerID, t.HITID })
                .Index(t => t.TaskChoiseID);
            
            AddColumn("dbo.WorkerHITs", "UserAnswer", c => c.String());
            DropColumn("dbo.TaskChoises", "Value");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TaskChoises", "Value", c => c.String());
            DropForeignKey("dbo.HITAnswers", new[] { "WorkerID", "HITID" }, "dbo.WorkerHITs");
            DropForeignKey("dbo.HITAnswers", "TaskChoiseID", "dbo.TaskChoises");
            DropIndex("dbo.HITAnswers", new[] { "TaskChoiseID" });
            DropIndex("dbo.HITAnswers", new[] { "WorkerID", "HITID" });
            DropColumn("dbo.WorkerHITs", "UserAnswer");
            DropTable("dbo.HITAnswers");
        }
    }
}
