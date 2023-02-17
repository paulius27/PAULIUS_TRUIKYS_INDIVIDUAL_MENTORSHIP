using BL.Models;
using DAL;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BL
{
    public class WeatherHistoryService : IWeatherHistoryService
    {
        private readonly IWeatherHistoryRepository _weatherHistoryRepository;
        private readonly IWeatherService _weatherService;

        public WeatherHistoryService(IWeatherHistoryRepository weatherHistoryRepository, IWeatherService weatherService) 
        {
            _weatherHistoryRepository = weatherHistoryRepository;
            _weatherService = weatherService;
        }

        public async Task UpdateWeatherHistory(IEnumerable<string> cityNames)
        {
            var requests = new List<Task<Weather>>();

            foreach (var cityName in cityNames)
                requests.Add(_weatherService.GetWeatherByCityNameAsync(cityName));

            var weatherResults = await Task.WhenAll(requests);

            var weatherHistoryEntries = weatherResults
                .Select(weather => new WeatherHistoryEntry 
                    {
                        CityName = weather.CityName,
                        Temperature = weather.Temperature,
                        Time = DateTime.Now
                    }
                );

            await _weatherHistoryRepository.InsertMany(weatherHistoryEntries);
        }
    }
}
