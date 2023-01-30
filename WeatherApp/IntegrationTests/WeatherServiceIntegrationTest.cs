using BL.Validation;
using BL;
using DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

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
            var geocodingRepository = new GeocodingRepository(httpClientFactory, apiKey);
            var weatherRepository = new WeatherRepository(httpClientFactory, apiKey);
            _weatherService = new WeatherService(geocodingRepository, weatherRepository, cityNameValidator, forecastDaysValidator);
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

            Assert.That(weatherDescription, Does.Match("In " + cityName + " -?(\\d+(?:[\\.\\,]\\d{1,2})?) °C\\. ([^.]+)\\."));
        }

        [Test]
        public async Task GetForecastDescriptionByCityNameAsync_GetCoordinatesFail_ErrorMessage()
        {
            string cityName = "?";

            var forecstDescription = await _weatherService.GetForecastDescriptionByCityNameAsync(cityName, 1);

            Assert.That(forecstDescription, Does.Match("Error: failed to get weather forecast data \\((.*?)\\)\\."));
        }

        [Test]
        public async Task GetForecastDescriptionByCityNameAsync_GetForecastSuccess_ForecastDescription()
        {
            string cityName = "Paris";

            var weatherDescription = await _weatherService.GetForecastDescriptionByCityNameAsync(cityName, 1);

            Assert.That(weatherDescription, Does.Match(cityName + " weather forecast:(\r\n|\r|\n)Day 1: -?(\\d+(?:[\\.\\,]\\d{1,2})?) °C\\. ([^.]+)\\."));
        }
    }
}