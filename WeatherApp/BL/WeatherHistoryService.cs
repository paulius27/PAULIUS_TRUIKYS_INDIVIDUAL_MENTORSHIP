using BL.Models;
using DAL;
using DAL.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BL
{
    public class WeatherHistoryService : IWeatherHistoryService
    {
        private readonly ILogger _logger;
        private readonly IWeatherHistoryRepository _weatherHistoryRepository;
        private readonly IWeatherService _weatherService;

        public WeatherHistoryService(ILogger<WeatherHistoryService> logger, IWeatherHistoryRepository weatherHistoryRepository, IWeatherService weatherService) 
        {
            _logger = logger;
            _weatherHistoryRepository = weatherHistoryRepository;
            _weatherService = weatherService;
        }

        public async Task UpdateWeatherHistory(params string[] cityNames)
        {
            var requests = new List<Task<Weather?>>();

            foreach (var cityName in cityNames)
                requests.Add(TryGetCurrentWeather(cityName));

            var weatherResults = await Task.WhenAll(requests);

            var weatherHistoryEntries = weatherResults
                .Where(weather => weather != null)
                .Select(weather => new WeatherHistoryEntry
                    {
                        CityName = weather.CityName,
                        Temperature = weather.Temperature,
                        Time = DateTime.Now
                    }
                );

            await _weatherHistoryRepository.InsertMany(weatherHistoryEntries);
        }

        private async Task<Weather?> TryGetCurrentWeather(string cityName)
        {
            try
            {
                return await _weatherService.GetWeatherByCityNameAsync(cityName);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to get current weather for city: {0}.", cityName);
                return null;
            }
        }
    }
}
