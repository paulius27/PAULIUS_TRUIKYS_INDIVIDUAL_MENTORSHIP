using BL.Models;
using DAL.Models;
using System.Threading.Tasks;

namespace BL
{
    public  interface IWeatherHistoryService
    {
        Task<WeatherHistory> GetWeatherHistory(string cityName, TimeRange timeRange);

        Task UpdateWeatherHistory(params string[] cityNames);
    }
}