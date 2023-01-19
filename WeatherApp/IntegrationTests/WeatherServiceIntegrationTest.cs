using BL.Validation;
using BL;
using DAL;
using Microsoft.Extensions.Configuration;

namespace IntegrationTests
{
    public class WeatherServiceIntegrationTest
    {
        private IWeatherService _weatherService;

        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<WeatherServiceIntegrationTest>()
                .Build();

            string apiKey = config["weather_api_key"] ?? throw new KeyNotFoundException("Weather API Key not found.");

            var validationService = new Validation();
            var weatherRepository = new WeatherRepository(apiKey);
            _weatherService = new WeatherService(weatherRepository, validationService);
        }

        [Test]
        public async Task GetWeatherDescriptionByCityNameAsync_GetTemperatureFail_ErrorMessage()
        {
            string cityName = "?";

            var weatherDescription = await _weatherService.GetWeatherDescriptionByCityNameAsync(cityName);

            Assert.That(weatherDescription, Does.Match("Error: failed to get weather data \\((.*?)\\)\\."));
        }

        [Test]
        public async Task GetWeatherDescriptionByCityNameAsync_GetTemperatureSuccess_WeatherDescription()
        {
            string cityName = "Paris";

            var weatherDescription = await _weatherService.GetWeatherDescriptionByCityNameAsync(cityName);

            Assert.That(weatherDescription, Does.Match("In " + cityName + " (\\d+(?:[\\.\\,]\\d{1,2})?) °C. ([^.]+)\\."));
        }
    }
}