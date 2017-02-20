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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public DbSet<Agency> Agency { get; set; }

        public DbSet<Deducted> Deducted { get; set; }

        public DbSet<Generation_buckle> Generation_buckle { get; set; }

        public DbSet<Generation_gives> Generation_gives { get; set; }

        public DbSet<Staff> Staff { get; set; }
    }
}
