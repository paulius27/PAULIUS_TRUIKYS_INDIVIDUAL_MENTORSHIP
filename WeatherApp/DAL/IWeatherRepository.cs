using DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DAL
{
    public interface IWeatherRepository
    {
        Task<double> GetTemperatureByCityNameAsync(string cityName);

        Task<double> GetTemperatureByCityNameAsync(string cityName, CancellationToken cancellationToken);

        Task<IEnumerable<WeatherForecast>> GetForecastByCoordinatesAsync(Coordinates coordinates, DateTime startDate, DateTime endDate);
    }
}
