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

        public DbSet<HomeStuff.Models.Item> Item { get; set; } = default!;

        public DbSet<HomeStuff.Models.Location> Location { get; set; } = default!;

        public DbSet<HomeStuff.Models.Maintenance> Maintenance { get; set; } = default!;

        public DbSet<HomeStuff.Models.ItemSet> ItemSet { get; set; } = default!;
    }
}
