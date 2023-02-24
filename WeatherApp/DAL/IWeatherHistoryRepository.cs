using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IWeatherHistoryRepository
    {
        Task InsertMany(IEnumerable<WeatherHistoryEntry> weatherHistoryEntries);
    }
}
