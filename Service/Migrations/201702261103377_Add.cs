namespace Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Generation_gives", "salesman_hiredate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Generation_gives", "salesman_hiredate");
        }
    }
}
