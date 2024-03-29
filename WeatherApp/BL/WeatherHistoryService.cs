﻿using BL.Models;
using BL.Validation;
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
        private readonly ICityService _cityService;
        private readonly IWeatherHistoryRepository _weatherHistoryRepository;
        private readonly IWeatherService _weatherService;
        private readonly IValidator<TimeRange> _timeRangeValidator;

        public WeatherHistoryService(ILogger<WeatherHistoryService> logger, ICityService cityService, IWeatherHistoryRepository weatherHistoryRepository, IWeatherService weatherService, IValidator<TimeRange> timeRangeValidator) 
        {
            _logger = logger;
            _cityService = cityService;
            _weatherHistoryRepository = weatherHistoryRepository;
            _weatherService = weatherService;
            _timeRangeValidator = timeRangeValidator;
        }

        public async Task<WeatherHistory> GetWeatherHistory(string cityName, TimeRange timeRange)
        {
            if (!_timeRangeValidator.Validate(timeRange))
                throw new ArgumentException("time range is not valid");

            var city = await _cityService.FindCity(cityName);

            if (city == null)
                throw new KeyNotFoundException("city not found");

            var weatherHistoryEntries = await _weatherHistoryRepository.FindByCityIdAndTimeRange(city.Id, timeRange);

            var weatherHistoryData = weatherHistoryEntries
                .Select(w => new WeatherHistoryDataEntry(w.Temperature, w.Time))
                .ToList();

            return new WeatherHistory(city.Name, weatherHistoryData);
        }

        public async Task UpdateWeatherHistory(params string[] cityNames)
        {
            var cities = await _cityService.FindOrAddCities(cityNames);
            var requests = new List<Task<Weather?>>();

            foreach (var city in cities)
                requests.Add(TryGetCurrentWeather(city.Name));

            var weatherResults = await Task.WhenAll(requests);
            var weatherHistoryEntries = new List<WeatherHistoryEntry>();

            for (int i = 0; i < weatherResults.Length; i++)
            {
                var weather = weatherResults[i];

                if (weather == null)
                    continue;

                weatherHistoryEntries.Add(new WeatherHistoryEntry
                {
                    Temperature = weather.Temperature,
                    Time = DateTime.Now,
                    CityId = cities.ElementAt(i).Id
                });
            }

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
