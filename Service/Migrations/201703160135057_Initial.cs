namespace Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Generation_buckle", "salesman_code", c => c.String());
            AddColumn("dbo.Generation_buckle", "gather_serial_num", c => c.String());
            AddColumn("dbo.Generation_gives", "salesman_code", c => c.String());
            AddColumn("dbo.Generation_gives", "gather_serial_num", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Generation_gives", "gather_serial_num");
            DropColumn("dbo.Generation_gives", "salesman_code");
            DropColumn("dbo.Generation_buckle", "gather_serial_num");
            DropColumn("dbo.Generation_buckle", "salesman_code");
        }
    }
}
