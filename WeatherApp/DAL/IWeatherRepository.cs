using DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL
{
    public interface IWeatherRepository
    {
        public Task<double> GetTemperatureByCityNameAsync(string cityName);

        public Task<IEnumerable<WeatherForecast>> GetForecastByCoordinatesAsync(Coordinates coordinates, DateTime startDate, DateTime endDate);
    }
}
