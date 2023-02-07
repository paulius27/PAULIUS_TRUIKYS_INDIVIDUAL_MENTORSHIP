using BL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL
{
    public interface IWeatherService
    {
        Task<Weather> GetWeatherByCityNameAsync(string cityName);

        Task<WeatherForecast> GetForecastByCityNameAsync(string cityName, int days);

        Task<string> GetMaxTemperatureByCityNamesAsync(IEnumerable<string> cityNames);
    }
}
