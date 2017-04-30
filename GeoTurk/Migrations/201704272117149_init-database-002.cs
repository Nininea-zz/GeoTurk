namespace GeoTurk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initdatabase002 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HITs",
                c => new
                    {
                        HITID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 300),
                        Description = c.String(),
                        DurationInMinutes = c.Decimal(nullable: false, precision: 6, scale: 2),
                        Instuction = c.String(nullable: false),
                        RelatedFilePath = c.String(),
                        ExpireDate = c.DateTime(nullable: false),
                        AnswerType = c.Int(nullable: false),
                        ChoiseType = c.Int(),
                    })
                .PrimaryKey(t => t.HITID);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        TagID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.TagID);
            
            CreateTable(
                "dbo.TaskChoises",
                c => new
                    {
                        TaskChoiseID = c.Int(nullable: false, identity: true),
                        IsCorrect = c.Boolean(nullable: false),
                        Label = c.String(),
                        Value = c.String(),
                        HITID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TaskChoiseID)
                .ForeignKey("dbo.HITs", t => t.HITID, cascadeDelete: true)
                .Index(t => t.HITID);
            
            CreateTable(
                "dbo.HITTags",
                c => new
                    {
                        HITID = c.Int(nullable: false),
                        TagID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.HITID, t.TagID })
                .ForeignKey("dbo.HITs", t => t.HITID, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagID, cascadeDelete: true)
                .Index(t => t.HITID)
                .Index(t => t.TagID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskChoises", "HITID", "dbo.HITs");
            DropForeignKey("dbo.HITTags", "TagID", "dbo.Tags");
            DropForeignKey("dbo.HITTags", "HITID", "dbo.HITs");
            DropIndex("dbo.HITTags", new[] { "TagID" });
            DropIndex("dbo.HITTags", new[] { "HITID" });
            DropIndex("dbo.TaskChoises", new[] { "HITID" });
            DropTable("dbo.HITTags");
            DropTable("dbo.TaskChoises");
            DropTable("dbo.Tags");
            DropTable("dbo.HITs");
        }
    }
}
