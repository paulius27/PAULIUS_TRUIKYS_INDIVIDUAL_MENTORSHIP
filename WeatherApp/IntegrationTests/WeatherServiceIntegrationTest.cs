using BL.Validation;
using BL;
using DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace IntegrationTests
{
    public class WeatherServiceIntegrationTest
    {
        private IWeatherService _weatherService;

        [SetUp]
        public void Setup()
        {
            IHttpClientFactory httpClientFactory = null;
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    httpClientFactory = services.AddHttpClient().BuildServiceProvider().GetService<IHttpClientFactory>();
                })
                .Build();

            var config = new ConfigurationBuilder()
                .AddUserSecrets<WeatherServiceIntegrationTest>()
                .AddJsonFile("integrationsettings.json")
                .Build();

            string apiKey = config["weather_api_key"] ?? throw new KeyNotFoundException("Weather API Key not found.");

            var cityNameValidator = new CityNameValidator();
            var forecastDaysValidator = new ForecastDaysValidator(config);
            var geocodingRepository = new GeocodingRepository(config, httpClientFactory);
            var weatherRepository = new WeatherRepository(config, httpClientFactory);
            _weatherService = new WeatherService(config, geocodingRepository, weatherRepository, cityNameValidator, forecastDaysValidator);
        }

        [Test]
        public void GetWeatherByCityNameAsync_GetTemperatureFail_ErrorMessage()
        {
            var ex = Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                await _weatherService.GetWeatherByCityNameAsync("?");
            });

            Assert.That(ex.Message, Is.EqualTo("Response status code does not indicate success: 404 (Not Found)."));
        }

        [Test]
        public async Task GetWeatherByCityNameAsync_GetTemperatureSuccess_Weather()
        {
            string cityName = "Paris";

            var weather = await _weatherService.GetWeatherByCityNameAsync(cityName);

            Assert.That(weather.CityName, Is.EqualTo(cityName));
            Assert.That(weather.Temperature.ToString(CultureInfo.InvariantCulture), Does.Match("-?(\\d+(?:[\\.\\,]\\d{1,2})?)"));
            Assert.That(string.IsNullOrWhiteSpace(weather.Comment), Is.False);
        }

        [Test]
        public void GetForecastByCityNameAsync_GetCoordinatesFail_ErrorMessage()
        {
            string cityName = "?";

            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await _weatherService.GetForecastByCityNameAsync(cityName, 1);
            });

            Assert.That(ex.Message, Is.EqualTo("city coordinates not found"));
        }

        [Test]
        public async Task GetForecastByCityNameAsync_GetForecastSuccess_Forecast()
        {
            string cityName = "Paris";

            var forecast = await _weatherService.GetForecastByCityNameAsync(cityName, 1);
            var forecastDay = forecast.Days.FirstOrDefault();

            Assert.That(forecast.CityName, Is.EqualTo(cityName));
            Assert.That(forecast.Days.Count, Is.EqualTo(1));
            Assert.That(forecastDay.Date.Date, Is.EqualTo(DateTime.Now.AddDays(1).Date));
            Assert.That(forecastDay.Temperature.ToString(CultureInfo.InvariantCulture), Does.Match("-?(\\d+(?:[\\.\\,]\\d{1,2})?)"));
            Assert.That(string.IsNullOrWhiteSpace(forecastDay.Comment), Is.False);
        }

        [Test]
        public async Task GetMaxTemperatureByCityNamesAsync_GetTemperaturesFail_ErrorMessage()
        {
            var errorMessage = await _weatherService.GetMaxTemperatureByCityNamesAsync(new List<string> { "?" });

            Assert.That(errorMessage, Does.Match("Error, no successful requests\\. Failed requests count: 1, canceled: 0\\."));
        }

        [Test]
        public async Task GetMaxTemperatureByCityNamesAsync_GetTemperaturesSucess_MaxTemperatureResult()
        {
            var result = await _weatherService.GetMaxTemperatureByCityNamesAsync(new List<string> { "Berlin", "Sydney" });

            Assert.That(result, Does.Match("City with the highest temperature of -?(\\d+(?:[\\.\\,]\\d{1,2})?) °C: (Berlin|Sydney)\\. Successful requests count: 2, failed: 0, canceled: 0\\."));
        }
    }
}