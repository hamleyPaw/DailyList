namespace HamleyPaw.DailyList.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DailyListItemActions", "ActionDateTimeUtc", c => c.DateTime(nullable: false));
            AddColumn("dbo.DailyListItems", "CreatedDateTimeUtc", c => c.DateTime(nullable: false));
            AddColumn("dbo.DailyListItems", "CompletedDateTimeUtc", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DailyListItems", "CompletedDateTimeUtc");
            DropColumn("dbo.DailyListItems", "CreatedDateTimeUtc");
            DropColumn("dbo.DailyListItemActions", "ActionDateTimeUtc");
        }
    }
}
