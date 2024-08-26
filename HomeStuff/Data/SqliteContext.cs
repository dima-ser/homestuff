using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HomeStuff.Models;

namespace HomeStuff.Data
{
    public class SqliteContext : DbContext
    {
        public SqliteContext (DbContextOptions<SqliteContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // make ItemSet optional so that deleting a set doesn't cascade delete items in it, but instead sets ItemSetId to NULL
            modelBuilder.Entity<Item>()
                .HasOne(b => b.ItemSet)
                .WithMany(a => a.Items)
                .OnDelete(DeleteBehavior.SetNull);
        }
        public DbSet<HomeStuff.Models.Item> Item { get; set; } = default!;

        public DbSet<HomeStuff.Models.Location> Location { get; set; } = default!;

        public DbSet<HomeStuff.Models.Maintenance> Maintenance { get; set; } = default!;

        public DbSet<HomeStuff.Models.ItemSet> ItemSet { get; set; } = default!;
    }
}
