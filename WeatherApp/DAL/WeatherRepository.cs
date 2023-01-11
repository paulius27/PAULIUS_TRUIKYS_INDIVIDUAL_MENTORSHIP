using System;
using System.Threading.Tasks;

namespace DAL
{
    public class WeatherRepository : IWeatherRepository
    {
        private static readonly Random _random = new Random();

        public async Task<double> GetTemperatureByCityNameAsync(string cityName)
        {
            double temperature = _random.Next(-10, 40);
            return temperature;
        }
    }
}
