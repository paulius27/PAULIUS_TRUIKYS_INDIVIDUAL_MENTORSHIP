using DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL
{
    public interface IWeatherHistoryRepository
    {
        Task<IEnumerable<WeatherHistoryEntry>> FindByCityIdAndTimeRange(int cityId, TimeRange timeRange);

        Task InsertMany(IEnumerable<WeatherHistoryEntry> weatherHistoryEntries);
    }
}
