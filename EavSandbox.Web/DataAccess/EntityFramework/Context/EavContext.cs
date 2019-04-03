using System;
using Microsoft.EntityFrameworkCore;

namespace EavSandbox.Web.EntityFramework.Context
{
    public class EavContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }

        public DbSet<Entity> Entities { get; set; }

        public DbSet<EntityAttribute> Attributes { get; set; }

        public DbSet<AttributeValue> Values { get; set; }

        public EavContext(DbContextOptions options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AttributeValue>()
                .HasKey(c => new { c.EntityId, c.AttributeId });
        }
    }
}
