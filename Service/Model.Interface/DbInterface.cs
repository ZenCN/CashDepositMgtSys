namespace Service.Model.Interface
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public class DbInterface : DbContext
    {
        /* Enable-Migrations -ContextTypeName Service.Model.Interface.DbInterface -MigrationsDirectory Migrations.Interface
         * Add-Migration Ãû×Ö -ConfigurationTypeName Service.Migrations.Interface.Configuration
         * Update-Database -ConfigurationTypeName Service.Migrations.Interface.Configuration
         */

        public DbInterface()
            : base("name=DbInterface")
        {
        }

        public DbSet<INTERFACE_MIO_BATCH_BZJ> INTERFACE_MIO_BATCH_BZJ { get; set; }
        public DbSet<INTERFACE_MIO_LIST_BZJ> INTERFACE_MIO_LIST_BZJ { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<INTERFACE_MIO_BATCH_BZJ>()
                .Property(e => e.MioType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<INTERFACE_MIO_BATCH_BZJ>()
                .Property(e => e.FromSys)
                .IsUnicode(false);

            modelBuilder.Entity<INTERFACE_MIO_BATCH_BZJ>()
                .Property(e => e.FromBatchNo)
                .IsUnicode(false);

            modelBuilder.Entity<INTERFACE_MIO_BATCH_BZJ>()
                .Property(e => e.GenerateBy)
                .IsUnicode(false);

            modelBuilder.Entity<INTERFACE_MIO_BATCH_BZJ>()
                .Property(e => e.MioChannel)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<INTERFACE_MIO_BATCH_BZJ>()
                .Property(e => e.PlatBatchNo)
                .IsUnicode(false);

            modelBuilder.Entity<INTERFACE_MIO_BATCH_BZJ>()
                .Property(e => e.AppvBy)
                .IsUnicode(false);

            modelBuilder.Entity<INTERFACE_MIO_BATCH_BZJ>()
                .Property(e => e.SendFile)
                .IsUnicode(false);

            modelBuilder.Entity<INTERFACE_MIO_BATCH_BZJ>()
                .Property(e => e.SendBy)
                .IsUnicode(false);

            modelBuilder.Entity<INTERFACE_MIO_BATCH_BZJ>()
                .Property(e => e.RcvFile)
                .IsUnicode(false);

            modelBuilder.Entity<INTERFACE_MIO_BATCH_BZJ>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<INTERFACE_MIO_BATCH_BZJ>()
                .Property(e => e.Reserved)
                .IsUnicode(false);

            modelBuilder.Entity<INTERFACE_MIO_LIST_BZJ>()
                .Property(e => e.FromSys)
                .IsUnicode(false);

            modelBuilder.Entity<INTERFACE_MIO_LIST_BZJ>()
                .Property(e => e.FromBatchNo)
                .IsUnicode(false);

            modelBuilder.Entity<INTERFACE_MIO_LIST_BZJ>()
                .Property(e => e.FromUniqLine)
                .IsUnicode(false);

            modelBuilder.Entity<INTERFACE_MIO_LIST_BZJ>()
                .Property(e => e.ClicBranch)
                .IsUnicode(false);

            modelBuilder.Entity<INTERFACE_MIO_LIST_BZJ>()
                .Property(e => e.BankCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<INTERFACE_MIO_LIST_BZJ>()
                .Property(e => e.BankAcc)
                .IsUnicode(false);

            modelBuilder.Entity<INTERFACE_MIO_LIST_BZJ>()
                .Property(e => e.BankAccName)
                .IsUnicode(false);

            modelBuilder.Entity<INTERFACE_MIO_LIST_BZJ>()
                .Property(e => e.MioAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<INTERFACE_MIO_LIST_BZJ>()
                .Property(e => e.ProcStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<INTERFACE_MIO_LIST_BZJ>()
                .Property(e => e.MioStatusRemark)
                .IsUnicode(false);

            modelBuilder.Entity<INTERFACE_MIO_LIST_BZJ>()
                .Property(e => e.AccOpenProvince)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<INTERFACE_MIO_LIST_BZJ>()
                .Property(e => e.AccOpenCity)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<INTERFACE_MIO_LIST_BZJ>()
                .Property(e => e.AccOpenBank)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<INTERFACE_MIO_LIST_BZJ>()
                .Property(e => e.AccBookOrCard)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<INTERFACE_MIO_LIST_BZJ>()
                .Property(e => e.AccPersonOrCompany)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<INTERFACE_MIO_LIST_BZJ>()
                .Property(e => e.AccCurrencyType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<INTERFACE_MIO_LIST_BZJ>()
                .Property(e => e.ErrMsg)
                .IsUnicode(false);
        }
    }
}
