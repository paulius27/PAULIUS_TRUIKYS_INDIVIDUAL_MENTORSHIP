using DAL.Context;
using DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL
{
    public class WeatherHistoryRepository : IWeatherHistoryRepository
    {
        private readonly WeatherDbContext _context;

        public WeatherHistoryRepository(WeatherDbContext context)
        {
            _context = context;
        }

        public async Task InsertMany(IEnumerable<WeatherHistoryEntry> weatherHistoryEntries)
        {
            await _context.WeatherHistory.AddRangeAsync(weatherHistoryEntries);
            await _context.SaveChangesAsync();
        }
    }
}
