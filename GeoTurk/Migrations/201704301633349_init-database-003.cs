namespace GeoTurk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initdatabase003 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HITTags", "HITID", "dbo.HITs");
            DropForeignKey("dbo.HITTags", "TagID", "dbo.Tags");
            DropIndex("dbo.HITTags", new[] { "HITID" });
            DropIndex("dbo.HITTags", new[] { "TagID" });
            AddColumn("dbo.HITs", "Tags", c => c.String());
            AddColumn("dbo.HITs", "CreatorID", c => c.Int(nullable: false));
            AlterColumn("dbo.Tags", "Title", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("dbo.HITs", "CreatorID");
            AddForeignKey("dbo.HITs", "CreatorID", "dbo.Users", "Id");
            DropTable("dbo.HITTags");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.HITTags",
                c => new
                    {
                        HITID = c.Int(nullable: false),
                        TagID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.HITID, t.TagID });
            
            DropForeignKey("dbo.HITs", "CreatorID", "dbo.Users");
            DropIndex("dbo.HITs", new[] { "CreatorID" });
            AlterColumn("dbo.Tags", "Title", c => c.String());
            DropColumn("dbo.HITs", "CreatorID");
            DropColumn("dbo.HITs", "Tags");
            CreateIndex("dbo.HITTags", "TagID");
            CreateIndex("dbo.HITTags", "HITID");
            AddForeignKey("dbo.HITTags", "TagID", "dbo.Tags", "TagID", cascadeDelete: true);
            AddForeignKey("dbo.HITTags", "HITID", "dbo.HITs", "HITID", cascadeDelete: true);
        }
    }
}
