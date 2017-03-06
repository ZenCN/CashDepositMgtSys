namespace Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MioList", "result", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MioList", "result");
        }
    }
}
