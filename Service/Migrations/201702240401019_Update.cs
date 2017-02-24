namespace Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Generation_buckle", "salesman_cash_deposit", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.Generation_buckle", "salesman_subbank_name");
            DropColumn("dbo.Generation_buckle", "cash_deposit");
            DropColumn("dbo.Generation_buckle", "record_state");
            DropColumn("dbo.Generation_gives", "salesman_subbank_name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Generation_gives", "salesman_subbank_name", c => c.String());
            AddColumn("dbo.Generation_buckle", "record_state", c => c.Int(nullable: false));
            AddColumn("dbo.Generation_buckle", "cash_deposit", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Generation_buckle", "salesman_subbank_name", c => c.String());
            DropColumn("dbo.Generation_buckle", "salesman_cash_deposit");
        }
    }
}
