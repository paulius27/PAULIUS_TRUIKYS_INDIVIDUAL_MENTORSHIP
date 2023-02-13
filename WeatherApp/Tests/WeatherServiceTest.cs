using BL;
using BL.Validation;
using DAL;
using DAL.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Text.Json;

namespace Tests
{
    public class WeatherServiceTest
    {
        private Mock<IConfiguration> _config;
        private Mock<IGeocodingRepository> _geocodingRepository;
        private Mock<IWeatherRepository> _weatherRepository;
        private Mock<IValidator<string>> _cityNameValidator;
        private Mock<IValidator<int>> _forecastDaysValidator;

        private WeatherService _weatherService;

        [SetUp]
        public void Setup()
        {
            _config = new Mock<IConfiguration>();
            _geocodingRepository = new Mock<IGeocodingRepository>();
            _weatherRepository = new Mock<IWeatherRepository>();
            _cityNameValidator = new Mock<IValidator<string>>();
            _forecastDaysValidator = new Mock<IValidator<int>>();

            _weatherService = new WeatherService(_config.Object, _geocodingRepository.Object, _weatherRepository.Object, _cityNameValidator.Object, _forecastDaysValidator.Object);
        }

        [Test]
        public void GetWeatherByCityNameAsync_ValidationFail_ErrorMessage()
        {
            _cityNameValidator.Setup(v => v.Validate(It.IsAny<string>())).Returns(false);

            var ex = Assert.ThrowsAsync<ArgumentException>(async () => 
            {
                await _weatherService.GetWeatherByCityNameAsync(""); 
            });
            
            Assert.That(ex.Message, Is.EqualTo("city name is not valid (Parameter 'cityName')"));
        }

        [Test]
        public void GetWeatherByCityNameAsync_GetTemperatureFail_ErrorMessage()
        {
            _cityNameValidator.Setup(v => v.Validate(It.IsAny<string>())).Returns(true);
            _weatherRepository.Setup(w => w.GetTemperatureByCityNameAsync(It.IsAny<string>())).ThrowsAsync(new JsonException("error"));

            var ex = Assert.ThrowsAsync<JsonException>(async () =>
            {
                await _weatherService.GetWeatherByCityNameAsync("London");
            });

            Assert.That(ex.Message, Is.EqualTo("error"));
        }

        [Test]
        [TestCase(-5, "Dress warmly")]
        [TestCase(15, "It's fresh")]
        [TestCase(25, "Good weather")]
        [TestCase(35, "It's time to go to the beach")]
        public async Task GetWeatherByCityNameAsync_GetTemperatureSuccess_Weather(double temperature, string expectedTemperatureComment)
        {
            string cityName = "London";
            _cityNameValidator.Setup(v => v.Validate(It.IsAny<string>())).Returns(true);
            _weatherRepository.Setup(w => w.GetTemperatureByCityNameAsync(It.IsAny<string>())).ReturnsAsync(temperature);

            var weather = await _weatherService.GetWeatherByCityNameAsync(cityName);

            Assert.That(weather.CityName, Is.EqualTo(cityName));
            Assert.That(weather.Temperature, Is.EqualTo(temperature));
            Assert.That(weather.Comment, Is.EqualTo(expectedTemperatureComment));
        }

        [Test]
        public void GetForecastByCityNameAsync_CityValidationFail_ErrorMessage()
        {
            _cityNameValidator.Setup(v => v.Validate(It.IsAny<string>())).Returns(false);

            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _weatherService.GetForecastByCityNameAsync("", 1);
            });

            Assert.That(ex.Message, Is.EqualTo("city name is not valid (Parameter 'cityName')"));
        }

        [Test]
        public void GetForecastByCityNameAsync_DaysValidationFail_ErrorMessage()
        {
            _cityNameValidator.Setup(v => v.Validate(It.IsAny<string>())).Returns(true);
            _forecastDaysValidator.Setup(v => v.Validate(It.IsAny<int>())).Returns(false);

            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _weatherService.GetForecastByCityNameAsync("London", 1);
            });

            Assert.That(ex.Message, Is.EqualTo("forecast days are not valid (Parameter 'days')"));
        }

        [Test]
        public void GetForecastByCityNameAsync_GetCoordinatesFail_ErrorMessage()
        {
            _cityNameValidator.Setup(v => v.Validate(It.IsAny<string>())).Returns(true);
            _forecastDaysValidator.Setup(v => v.Validate(It.IsAny<int>())).Returns(true);
            _geocodingRepository.Setup(g => g.GetCoordinatesByCityNameAsync(It.IsAny<string>())).ThrowsAsync(new KeyNotFoundException("city coordinates not found"));

            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await _weatherService.GetForecastByCityNameAsync("London", 1);
            });

            Assert.That(ex.Message, Is.EqualTo("city coordinates not found"));
        }

        [Test]
        public async Task GetForecastByCityNameAsync_GetForecastSuccess_Forecast()
        {
            var cityName = "London";
            var date = new DateTime(2023, 1, 25);
            var forecasts = new List<WeatherForecastData>
            {
                new WeatherForecastData(date, 7, 9)
            };

            _cityNameValidator.Setup(v => v.Validate(It.IsAny<string>())).Returns(true);
            _forecastDaysValidator.Setup(v => v.Validate(It.IsAny<int>())).Returns(true);
            _geocodingRepository.Setup(g => g.GetCoordinatesByCityNameAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<Coordinates>());
            _weatherRepository.Setup(w => w.GetForecastByCoordinatesAsync(It.IsAny<Coordinates>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(forecasts);

            var forecast = await _weatherService.GetForecastByCityNameAsync(cityName, 1);
            var forecastDay = forecast.Days.FirstOrDefault();
            
            Assert.That(forecast.CityName, Is.EqualTo(cityName));
            Assert.That(forecast.Days.Count, Is.EqualTo(1));
            Assert.That(forecastDay?.Date, Is.EqualTo(date));
            Assert.That(forecastDay?.Temperature, Is.EqualTo(8));
            Assert.That(forecastDay?.Comment, Is.EqualTo("It's fresh"));
        }

        [Test]
        public async Task GetMaxTemperatureByCityNamesAsync_GetTemperaturesFail_ErrorMessage()
        {
            _cityNameValidator.Setup(v => v.Validate(It.IsAny<string>())).Returns(false);

            var errorMessage = await _weatherService.GetMaxTemperatureByCityNamesAsync(new List<string> { "", " " });

            Assert.That(errorMessage, Does.Match("Error, no successful requests\\. Failed requests count: 2, canceled: 0\\."));
        }

        [Test]
        public async Task GetMaxTemperatureByCityNamesAsync_GetTemperaturesTimeout_ErrorMessage()
        {
            _cityNameValidator.Setup(v => v.Validate(It.IsAny<string>())).Returns(true);
            _weatherRepository.Setup(w => w.GetTemperatureByCityNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ThrowsAsync(new TaskCanceledException());

            var result = await _weatherService.GetMaxTemperatureByCityNamesAsync(new List<string> { "Berlin", "Sydney" });

            Assert.That(result, Does.Match("Error, no successful requests\\. Failed requests count: 0, canceled: 2\\."));
        }

        [Test]
        public async Task GetMaxTemperatureByCityNamesAsync_GetTemperaturesSucess_MaxTemperatureResult()
        {
            _cityNameValidator.Setup(v => v.Validate(It.IsAny<string>())).Returns(true);
            _weatherRepository.Setup(w => w.GetTemperatureByCityNameAsync("Berlin", It.IsAny<CancellationToken>())).ReturnsAsync(5);
            _weatherRepository.Setup(w => w.GetTemperatureByCityNameAsync("Sydney", It.IsAny<CancellationToken>())).ReturnsAsync(20);

            var result = await _weatherService.GetMaxTemperatureByCityNamesAsync(new List<string> { "Berlin", "Sydney" });

            Assert.That(result, Does.Match("City with the highest temperature of 20 °C: Sydney\\. Successful requests count: 2, failed: 0, canceled: 0\\."));
        }
    }
}
