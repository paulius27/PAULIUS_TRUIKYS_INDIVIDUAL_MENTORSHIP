using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Models.OpenMeteo;
using DAL.Models.OpenWeather;

namespace DAL
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;

        public WeatherRepository(IHttpClientFactory httpClientFactory, string apiKey)
        {
            _httpClientFactory = httpClientFactory;
            _apiKey = apiKey;
        }

        public async Task<double> GetTemperatureByCityNameAsync(string cityName) => await GetTemperatureByCityNameAsync(cityName, CancellationToken.None);

        public async Task<double> GetTemperatureByCityNameAsync(string cityName, CancellationToken cancellationToken)
        {
            using var httpClient = _httpClientFactory.CreateClient();
            using var response = await httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/weather?q={cityName}&units=metric&APPID={_apiKey}", cancellationToken);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var weather = JsonSerializer.Deserialize<WeatherResponse>(responseBody, options);

            return weather.Main.Temp;
        }

        public async Task<IEnumerable<WeatherForecast>> GetForecastByCoordinatesAsync(Coordinates coordinates, DateTime startDate, DateTime endDate)
        {
            using var httpClient = _httpClientFactory.CreateClient();
            using var response = await httpClient.GetAsync($"https://api.open-meteo.com/v1/forecast?latitude={coordinates.Latitude.ToString(CultureInfo.InvariantCulture)}&longitude={coordinates.Longitude.ToString(CultureInfo.InvariantCulture)}&daily=temperature_2m_max,temperature_2m_min&timezone=auto&start_date={startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}&end_date={endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var weatherForecastResponse = JsonSerializer.Deserialize<WeatherForecastResponse>(responseBody, options);

            var weatherForecasts = new List<WeatherForecast>();
            for (var i = 0; i < weatherForecastResponse.Daily.Time.Count; i++)
            {
                var weatherForecast = new WeatherForecast(
                    DateTime.Parse(weatherForecastResponse.Daily.Time[i], CultureInfo.InvariantCulture), 
                    weatherForecastResponse.Daily.Temperature2mMin[i], 
                    weatherForecastResponse.Daily.Temperature2mMax[i]
                );

                weatherForecasts.Add(weatherForecast);
            }

            return weatherForecasts;
        }
    }
}
