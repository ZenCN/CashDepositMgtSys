using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Service.Model
{
    public class Db : DbContext
    {
        public Db()
            : base("name=DbContext")
        {
        }

        /* EntityFramework CodeFirst 数据库迁移语法
         * PM> Enable-Migrations -ContextTypeName [“绝对命名路径”即继承自DbContext的子类命名空间 + 该子类名] -MigrationsDirectory [生成Migration的文件夹名] -Force
         * PM> Add-Migration [迁移文件的名字] -ConfigurationTypeName [生成Migration的文件夹下的Configuration类的“绝对命名路径”]
         * PM> Update-Database -ConfigurationTypeName [生成Migration的文件夹下的Configuration类的“绝对命名路径”]
         * 注意：只有单个DbContext时上面可不用带参数
         */

        /*使用Code First做ORM，一旦数据库表有变化，再运行时必须做Code First数据库迁移，否会会报“无法检查模型兼容性...”错误：
         * PM> Enable-Migrations   启用Code First 迁移
         * 成功之后会生成Migrations文件夹、Migrations文件夹下的Configuration.cs，可在Configuration.cs中的Seed方法添加测试数据，然后在执行以下语句
         * PM> Add-Migration Initial  创建初始化迁移，Initial是随意命名的，用来命名创建好的迁移文件
         * 成功后，创建了{DateStamp}_Initial.cs迁移文件，它包含了为数据库创建相关表的指令，之后再执行以下语句
         * PM> Update-Database    更新数据库，如果要强行执行语句更新数据库则加个参数 -Force 即可
         */

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            /*用Entity Framework的 Fluent API方式配置Code First 指定数据库对应的表名、主键，也可以用Data Annotation注解方式设置映射约定
             * 使用Data Annotation注解方式，当数据库发生变化时，可不用做Code First数据库迁移
             */
            modelBuilder.Entity<Agency>().ToTable("Agency", "dbo");

            modelBuilder.Entity<Deducted>().ToTable("Deducted", "dbo").HasKey(t => t.id);

            modelBuilder.Entity<Generation_buckle>().ToTable("Generation_buckle", "dbo").HasKey(t => t.id);

            modelBuilder.Entity<Generation_gives>().ToTable("Generation_gives", "dbo").HasKey(t => t.id);

            modelBuilder.Entity<Staff>().ToTable("Staff", "dbo").HasKey(t => t.id);

            modelBuilder.Entity<MioBatch>().ToTable("MioBatch", "dbo").HasKey(t => t.id);

            modelBuilder.Entity<MioList>().ToTable("MioList", "dbo").HasKey(t => t.id);
        }

        public DbSet<Agency> Agency { get; set; }

        public DbSet<Deducted> Deducted { get; set; }

        public DbSet<Generation_buckle> Generation_buckle { get; set; }

        public DbSet<Generation_gives> Generation_gives { get; set; }

        public DbSet<Staff> Staff { get; set; }

        public DbSet<MioBatch> MioBatche { get; set; }

        public DbSet<MioList> MioList { get; set; }
    }
}