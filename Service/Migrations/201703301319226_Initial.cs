namespace Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        type = c.String(),
                        number = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Agency",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        code = c.String(),
                        p_code = c.String(),
                        name = c.String(),
                        type = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Deducted",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        generation_gives_id = c.Int(),
                        item = c.String(),
                        amount = c.Decimal(precision: 18, scale: 2),
                        remark = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Generation_buckle",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        record_date = c.DateTime(),
                        recorder_code = c.String(),
                        reviewer_code = c.String(),
                        review_state = c.Int(),
                        review_date = c.DateTime(),
                        agency_code = c.String(),
                        salesman_name = c.String(),
                        salesman_sex = c.String(),
                        salesman_code = c.String(),
                        gather_serial_num = c.String(),
                        salesman_card_type = c.String(),
                        salesman_card_id = c.String(),
                        salesman_phone = c.String(),
                        salesman_hiredate = c.DateTime(),
                        salesman_bank_account_name = c.String(),
                        salesman_bank_account_number = c.String(),
                        salesman_bank_name = c.String(),
                        salesman_bank_province = c.String(),
                        salesman_bank_city = c.String(),
                        salesman_cash_deposit = c.Decimal(precision: 18, scale: 2),
                        remark = c.String(),
                        is_deleted = c.Int(),
                        channel = c.String(),
                        process_result = c.String(),
                        finish_time = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Generation_gives",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        record_date = c.DateTime(),
                        recorder_code = c.String(),
                        reviewer_code = c.String(),
                        review_state = c.Int(),
                        review_date = c.DateTime(),
                        agency_code = c.String(),
                        salesman_name = c.String(),
                        salesman_sex = c.String(),
                        salesman_code = c.String(),
                        gather_serial_num = c.String(),
                        salesman_card_type = c.String(),
                        salesman_card_id = c.String(),
                        salesman_phone = c.String(),
                        salesman_hiredate = c.DateTime(),
                        salesman_bank_account_name = c.String(),
                        salesman_bank_account_number = c.String(),
                        salesman_bank_name = c.String(),
                        salesman_bank_province = c.String(),
                        salesman_bank_city = c.String(),
                        salesman_cash_deposit = c.Decimal(precision: 18, scale: 2),
                        salesman_refunds = c.Decimal(precision: 18, scale: 2),
                        salesman_refunds_state = c.Int(),
                        remark = c.String(),
                        is_deleted = c.Int(),
                        channel = c.String(),
                        process_result = c.String(),
                        finish_time = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.MioBatch",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        batch_id = c.String(),
                        record_count = c.Int(nullable: false),
                        sum_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        mio_type = c.String(),
                        reviewer_code = c.String(),
                        review_date = c.DateTime(),
                        push_result = c.String(),
                        push_date = c.DateTime(),
                        done_date = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.MioList",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        batch_id = c.String(),
                        generation_id = c.Int(nullable: false),
                        mio_type = c.String(),
                        bank_account_name = c.String(),
                        bank_account_no = c.String(),
                        cash_deposit = c.Decimal(precision: 18, scale: 2),
                        status = c.Int(),
                        result = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Staff",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        code = c.String(),
                        role = c.String(),
                        authority = c.Int(),
                        jurisdiction = c.String(),
                        agency_code = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Staff");
            DropTable("dbo.MioList");
            DropTable("dbo.MioBatch");
            DropTable("dbo.Generation_gives");
            DropTable("dbo.Generation_buckle");
            DropTable("dbo.Deducted");
            DropTable("dbo.Agency");
            DropTable("dbo.Account");
        }
    }
}
