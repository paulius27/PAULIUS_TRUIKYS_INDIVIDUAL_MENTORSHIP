using BL.Models;
using System;
using System.Threading.Tasks;

namespace BL
{
    public  interface IWeatherHistoryService
    {
        Task<WeatherHistory> GetWeatherHistory(string cityName, DateTime from, DateTime to);

        Task UpdateWeatherHistory(params string[] cityNames);
    }
}