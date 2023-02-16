using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    public class WeatherDbContext : DbContext
    {
        public WeatherDbContext(DbContextOptions<WeatherDbContext> options) : base(options) { }

        public DbSet<WeatherHistoryEntry> WeatherHistory { get; set; }
    }
}
