namespace Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _228 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Generation_buckle", "channel", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Generation_buckle", "channel");
        }
    }
}
