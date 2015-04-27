namespace HamleyPaw.DailyList.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetCompletedDateTimeToNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DailyListItems", "CompletedDateTimeUtc", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DailyListItems", "CompletedDateTimeUtc", c => c.DateTime(nullable: false));
        }
    }
}
