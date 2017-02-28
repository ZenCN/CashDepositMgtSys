namespace Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_channel_field : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Generation_gives", "channel", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Generation_gives", "channel");
        }
    }
}
