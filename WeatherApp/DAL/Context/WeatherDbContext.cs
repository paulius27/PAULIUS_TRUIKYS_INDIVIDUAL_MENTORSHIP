using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    public class WeatherDbContext : DbContext
    {
        public WeatherDbContext(DbContextOptions<WeatherDbContext> options) : base(options) { }

        public DbSet<City> Cities { get; set; }

        public DbSet<WeatherHistoryEntry> WeatherHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeatherHistoryEntry>()
                .HasOne(w => w.City)
                .WithMany(c => c.WeatherHistory)
                .HasForeignKey(w => w.CityId);
        }
    }
}
