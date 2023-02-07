using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BL.Models;
using BL.Validation;
using DAL;
using Microsoft.Extensions.Configuration;

namespace BL
{
    public class WeatherService : IWeatherService
    {
        private readonly IGeocodingRepository _geocodingRepository;
        private readonly IWeatherRepository _weatherRepository;
        private readonly IValidator<string> _cityNameValidator;
        private readonly IValidator<int> _forecastDaysValidator;

        private readonly bool _showDebugInfo;
        private readonly int _findMaxTemperatureTimeoutMs;

        public WeatherService(IConfiguration config, IGeocodingRepository geocodingRepository, IWeatherRepository weatherRepository, IValidator<string> cityNameValidator, IValidator<int> forecastDaysValidator)
        {
            _geocodingRepository = geocodingRepository;
            _weatherRepository = weatherRepository;
            _cityNameValidator = cityNameValidator;
            _forecastDaysValidator = forecastDaysValidator;

            if (!bool.TryParse(config["FindMaxTemperature:ShowDebugInfo"], out _showDebugInfo))
                _showDebugInfo = true;

            if (!int.TryParse(config["FindMaxTemperature:TimeoutMs"], out _findMaxTemperatureTimeoutMs))
                _findMaxTemperatureTimeoutMs = 5000;
        }

        public async Task<Weather> GetWeatherByCityNameAsync(string cityName)
        {
            //if (!_cityNameValidator.Validate(cityName))
            //    return "Error: city name is not valid.";
            
            //try
            //{
        
            double temperature = await _weatherRepository.GetTemperatureByCityNameAsync(cityName);

            return new Weather(cityName, temperature, GetTemperatureComment(temperature));

            //}
            //catch (Exception ex)
            //{
            //    return $"Error: failed to get weather data ({ex.Message}).";
            //}
        }

        public async Task<string> GetForecastDescriptionByCityNameAsync(string cityName, int days)
        {
            if (!_cityNameValidator.Validate(cityName))
                return "Error: city name is not valid.";

            if (!_forecastDaysValidator.Validate(days))
                return "Error: forecast days are not valid.";

            try
            {
                var coordinates = await _geocodingRepository.GetCoordinatesByCityNameAsync(cityName);

                DateTime startDate = DateTime.Now.AddDays(1);
                DateTime endDate = startDate.AddDays(days - 1);
                var forecasts = await _weatherRepository.GetForecastByCoordinatesAsync(coordinates, startDate, endDate);

                var i = 0;
                var sb = new StringBuilder($"{cityName} weather forecast:");

                foreach (var forecast in forecasts)
                {
                    double temperature = Math.Round((forecast.MinTemperature + forecast.MaxTemperature) / 2, 2);

                    i++;
                    sb.AppendLine();
                    sb.Append($"Day {i}: {temperature} °C. {GetTemperatureComment(temperature)}.");
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                return $"Error: failed to get weather forecast data ({ex.Message}).";
            }
        }

        public async Task<string> GetMaxTemperatureByCityNamesAsync(IEnumerable<string> cityNames)
        {
            var cancellationTokenSource = new CancellationTokenSource(_findMaxTemperatureTimeoutMs);
            var requests = new List<Task<(double? Temperature, string DebugInfo, bool Canceled)>>();

            foreach (var cityName in cityNames) 
                requests.Add(GetTemperatureWithDebugInfoAsync(cityName, cancellationTokenSource.Token));

            var results = await Task.WhenAll(requests);

            var maxTemperature = double.MinValue;
            var maxTemperatureIndex = -1;
            var successfulRequests = 0;
            var cancelledRequests = 0;

            for (var i = 0; i < results.Length; i++)
            {
                var result = results[i];

                if (result.Canceled)
                    cancelledRequests++;

                if (result.Temperature == null)
                    continue;
                
                successfulRequests++;

                if (result.Temperature > maxTemperature)
                {
                    maxTemperature = (double)result.Temperature;
                    maxTemperatureIndex = i;
                }
            }

            var failedRequests = requests.Count - successfulRequests - cancelledRequests;
            var sb = new StringBuilder();

            if (successfulRequests > 0) 
                sb.Append($"City with the highest temperature of {maxTemperature} °C: {cityNames.ElementAt(maxTemperatureIndex)}. Successful requests count: {successfulRequests}, failed: {failedRequests}, canceled: {cancelledRequests}.");
            else
                sb.Append($"Error, no successful requests. Failed requests count: {failedRequests}, canceled: {cancelledRequests}.");

            if (_showDebugInfo) 
            {
                foreach (var result in results)
                {
                    sb.AppendLine();
                    sb.Append(result.DebugInfo);
                }
            }

            return sb.ToString();
        }

        private async Task<(double? Temperature, string DebugInfo, bool Cancelled)> GetTemperatureWithDebugInfoAsync(string cityName, CancellationToken cancellationToken)
        {
            Stopwatch sw = Stopwatch.StartNew();

            if (!_cityNameValidator.Validate(cityName))
                return (null, $"City: {cityName}. Error: city name is not valid. Timer: {sw.Elapsed.TotalMilliseconds} ms.", false);

            try
            {
                double temperature = await _weatherRepository.GetTemperatureByCityNameAsync(cityName, cancellationToken);
                return (temperature, $"City: {cityName}. Temperature: {temperature} °C. Timer: {sw.Elapsed.TotalMilliseconds} ms.", false);
            }
            catch (TaskCanceledException)
            {
                return (null, $"Weather request for {cityName} was canceled due to a timeout.", true);
            }
            catch (Exception ex)
            {
                return (null, $"City: {cityName}. Error: failed to get weather data ({ex.Message}). Timer: {sw.Elapsed.TotalMilliseconds} ms.", false);
            }
        }

        private string GetTemperatureComment(double temperature)
        {
            if (temperature < 0)
                return "Dress warmly";
            else if (temperature < 20)
                return "It's fresh";
            else if (temperature < 30)
                return "Good weather";
            else
                return "It's time to go to the beach";
        }
    }
}
