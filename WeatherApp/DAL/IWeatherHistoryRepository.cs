using DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL
{
    public interface IWeatherHistoryRepository
    {
        Task<IEnumerable<WeatherHistoryEntry>> FindByCityIdAndTimeRange(int cityId, DateTime from, DateTime to);

        Task InsertMany(IEnumerable<WeatherHistoryEntry> weatherHistoryEntries);
    }
}
