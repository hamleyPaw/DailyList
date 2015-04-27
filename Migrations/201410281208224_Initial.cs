namespace HamleyPaw.DailyList.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DailyListItemActions",
                c => new
                    {
                        DailyListItemActionId = c.Guid(nullable: false),
                        DailyListItemId = c.Guid(nullable: false),
                        Type = c.Int(nullable: false),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.DailyListItemActionId)
                .ForeignKey("dbo.DailyListItems", t => t.DailyListItemId, cascadeDelete: true)
                .Index(t => t.DailyListItemId);
            
            CreateTable(
                "dbo.DailyListItems",
                c => new
                    {
                        DailyListItemId = c.Guid(nullable: false),
                        ItemText = c.String(maxLength: 50),
                        IsRunning = c.Boolean(nullable: false),
                        IsComplete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.DailyListItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DailyListItemActions", "DailyListItemId", "dbo.DailyListItems");
            DropIndex("dbo.DailyListItemActions", new[] { "DailyListItemId" });
            DropTable("dbo.DailyListItems");
            DropTable("dbo.DailyListItemActions");
        }
    }
}
