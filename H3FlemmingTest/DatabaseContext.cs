using H3FlemmingTest.Models;
using Microsoft.EntityFrameworkCore;

namespace H3FlemmingTest
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> option)
        : base(option)
        {
        }

        // tables in DB
        public DbSet<Hall> Hall { get; set; }
        public DbSet<Seat> Seat { get; set; }
        public DbSet<Reserved> Reserved { get; set; }
    }
}
