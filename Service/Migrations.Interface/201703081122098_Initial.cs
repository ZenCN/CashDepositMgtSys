namespace Service.Migrations.Interface
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.INTERFACE_MIO_BATCH_BZJ",
                c => new
                    {
                        BatchId = c.Int(nullable: false, identity: true),
                        MioType = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        FromSys = c.String(nullable: false, maxLength: 10, unicode: false),
                        FromBatchNo = c.String(nullable: false, maxLength: 30, unicode: false),
                        DataCnt = c.Int(nullable: false),
                        SumAmnt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BatchStatus = c.Int(nullable: false),
                        GenerateTime = c.DateTime(nullable: false),
                        GenerateBy = c.String(nullable: false, maxLength: 20, unicode: false),
                        MioChannel = c.String(maxLength: 2, fixedLength: true, unicode: false),
                        PlatBatchNo = c.String(maxLength: 50, unicode: false),
                        AppvTime = c.DateTime(),
                        AppvBy = c.String(maxLength: 20, unicode: false),
                        SendTime = c.DateTime(),
                        SendFile = c.String(maxLength: 50, unicode: false),
                        SendBy = c.String(maxLength: 20, unicode: false),
                        RcvFile = c.String(maxLength: 50, unicode: false),
                        RcvTime = c.DateTime(),
                        Remark = c.String(maxLength: 100, unicode: false),
                        Reserved = c.String(maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.BatchId);
            
            CreateTable(
                "dbo.INTERFACE_MIO_LIST_BZJ",
                c => new
                    {
                        LineId = c.Long(nullable: false, identity: true),
                        FromSys = c.String(nullable: false, maxLength: 10, unicode: false),
                        FromBatchNo = c.String(nullable: false, maxLength: 50, unicode: false),
                        FromUniqLine = c.String(nullable: false, maxLength: 30, unicode: false),
                        ClicBranch = c.String(nullable: false, maxLength: 6, unicode: false),
                        BankCode = c.String(nullable: false, maxLength: 4, fixedLength: true, unicode: false),
                        BankAcc = c.String(nullable: false, maxLength: 30, unicode: false),
                        BankAccName = c.String(nullable: false, maxLength: 40, unicode: false),
                        MioAmount = c.Decimal(nullable: false, storeType: "money"),
                        BatchId = c.Long(),
                        ApplTime = c.DateTime(nullable: false),
                        TreatTime = c.DateTime(),
                        FinishTime = c.DateTime(),
                        ProcStatus = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        MioStatus = c.Int(nullable: false),
                        MioStatusRemark = c.String(maxLength: 200, unicode: false),
                        AccOpenProvince = c.String(maxLength: 20, fixedLength: true, unicode: false),
                        AccOpenCity = c.String(maxLength: 20, fixedLength: true, unicode: false),
                        AccOpenBank = c.String(maxLength: 60, fixedLength: true, unicode: false),
                        AccBookOrCard = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        AccPersonOrCompany = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        AccCurrencyType = c.String(nullable: false, maxLength: 3, fixedLength: true, unicode: false),
                        ErrMsg = c.String(maxLength: 256, unicode: false),
                    })
                .PrimaryKey(t => t.LineId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.INTERFACE_MIO_LIST_BZJ");
            DropTable("dbo.INTERFACE_MIO_BATCH_BZJ");
        }
    }
}
