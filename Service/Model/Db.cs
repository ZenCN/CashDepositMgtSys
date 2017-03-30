using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Service.Model
{
    public class Db : DbContext
    {
        /* Enable-Migrations -ContextTypeName Service.Model.Db
         * Add-Migration 名字 -ConfigurationTypeName Service.Migrations.Configuration
         * Update-Database -ConfigurationTypeName Service.Migrations.Configuration
         */

        public Db()
            : base("name=DbContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            /*用Entity Framework的 Fluent API方式配置Code First 指定数据库对应的表名、主键，也可以用Data Annotation注解方式设置映射约定
             * 使用Data Annotation注解方式，当数据库发生变化时，可不用做Code First数据库迁移
             */
            modelBuilder.Entity<Account>().ToTable("Account", "dbo");

            modelBuilder.Entity<Agency>().ToTable("Agency", "dbo");

            modelBuilder.Entity<Deducted>().ToTable("Deducted", "dbo").HasKey(t => t.id);

            modelBuilder.Entity<Generation_buckle>().ToTable("Generation_buckle", "dbo").HasKey(t => t.id);

            modelBuilder.Entity<Generation_gives>().ToTable("Generation_gives", "dbo").HasKey(t => t.id);

            modelBuilder.Entity<Staff>().ToTable("Staff", "dbo").HasKey(t => t.id);

            modelBuilder.Entity<MioBatch>().ToTable("MioBatch", "dbo").HasKey(t => t.id);

            modelBuilder.Entity<MioList>().ToTable("MioList", "dbo").HasKey(t => t.id);
        }

        public DbSet<Account> Account { get; set; }

        public DbSet<Agency> Agency { get; set; }

        public DbSet<Deducted> Deducted { get; set; }

        public DbSet<Generation_buckle> Generation_buckle { get; set; }

        public DbSet<Generation_gives> Generation_gives { get; set; }

        public DbSet<Staff> Staff { get; set; }

        public DbSet<MioBatch> MioBatch { get; set; }

        public DbSet<MioList> MioList { get; set; }
    }
}