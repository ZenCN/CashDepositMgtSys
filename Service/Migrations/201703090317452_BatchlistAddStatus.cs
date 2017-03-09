namespace Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BatchlistAddStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MioList", "status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MioList", "status");
        }
    }
}
