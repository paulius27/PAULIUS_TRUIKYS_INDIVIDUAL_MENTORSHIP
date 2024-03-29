﻿using DAL.Context;
using DAL.Models;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class WeatherHistoryRepository : IWeatherHistoryRepository
    {
        private readonly WeatherDbContext _context;

        public WeatherHistoryRepository(WeatherDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WeatherHistoryEntry>> FindByCityIdAndTimeRange(int cityId, TimeRange timeRange)
        {
            var weatherHistory = await _context.WeatherHistory
                .Where(wh => wh.CityId == cityId)
                .Where(wh => wh.Time >= timeRange.Start && wh.Time <= timeRange.End)
                .ToListAsync();

            return weatherHistory;
        }

        public async Task InsertMany(IEnumerable<WeatherHistoryEntry> weatherHistoryEntries)
        {
            await _context.WeatherHistory.AddRangeAsync(weatherHistoryEntries);
            await _context.SaveChangesAsync();
        }
    }
}
