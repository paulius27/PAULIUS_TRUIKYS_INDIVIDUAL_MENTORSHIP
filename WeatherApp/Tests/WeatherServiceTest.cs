using BL;
using BL.Validation;
using DAL;
using DAL.Models;
using Moq;
using System.Text.Json;

namespace Tests
{
    public class WeatherServiceTest
    {
        private Mock<IGeocodingRepository> _geocodingRepository;
        private Mock<IWeatherRepository> _weatherRepository;
        private Mock<IValidator<string>> _cityNameValidator;
        private Mock<IValidator<int>> _forecastDaysValidator;

        private WeatherService _weatherService;

        [SetUp]
        public void Setup()
        {
            _geocodingRepository = new Mock<IGeocodingRepository>();
            _weatherRepository = new Mock<IWeatherRepository>();
            _cityNameValidator = new Mock<IValidator<string>>();
            _forecastDaysValidator = new Mock<IValidator<int>>();

            _weatherService = new WeatherService(_geocodingRepository.Object, _weatherRepository.Object, _cityNameValidator.Object, _forecastDaysValidator.Object);
        }

        [Test]
        public async Task GetWeatherDescriptionByCityNameAsync_ValidationFail_ErrorMessage()
        {
            _cityNameValidator.Setup(v => v.Validate(It.IsAny<string>())).Returns(false);

            var weatherDescription = await _weatherService.GetWeatherDescriptionByCityNameAsync("");

            Assert.That(weatherDescription, Is.EqualTo("Error: city name is not valid."));
        }

        [Test]
        public async Task GetWeatherDescriptionByCityNameAsync_GetTemperatureFail_ErrorMessage()
        {
            _cityNameValidator.Setup(v => v.Validate(It.IsAny<string>())).Returns(true);
            _weatherRepository.Setup(w => w.GetTemperatureByCityNameAsync(It.IsAny<string>())).ThrowsAsync(new JsonException("error"));

            var weatherDescription = await _weatherService.GetWeatherDescriptionByCityNameAsync("London");

            Assert.That(weatherDescription, Is.EqualTo("Error: failed to get weather data (error)."));
        }

        [Test]
        [TestCase(-5, "Dress warmly")]
        [TestCase(15, "It's fresh")]
        [TestCase(25, "Good weather")]
        [TestCase(35, "It's time to go to the beach")]
        public async Task GetWeatherDescriptionByCityNameAsync_GetTemperatureSuccess_WeatherDescription(double temperature, string expectedTemperatureComment)
        {
            string cityName = "London";
            _cityNameValidator.Setup(v => v.Validate(It.IsAny<string>())).Returns(true);
            _weatherRepository.Setup(w => w.GetTemperatureByCityNameAsync(It.IsAny<string>())).ReturnsAsync(temperature);

            var weatherDescription = await _weatherService.GetWeatherDescriptionByCityNameAsync(cityName);

            Assert.That(weatherDescription, Is.EqualTo($"In {cityName} {temperature} °C. {expectedTemperatureComment}."));
        }

        [Test]
        public async Task GetForecastDescriptionByCityNameAsync_CityValidationFail_ErrorMessage()
        {
            _cityNameValidator.Setup(v => v.Validate(It.IsAny<string>())).Returns(false);

            var forecastDescription = await _weatherService.GetForecastDescriptionByCityNameAsync("", 1);

            Assert.That(forecastDescription, Is.EqualTo("Error: city name is not valid."));
        }

        [Test]
        public async Task GetForecastDescriptionByCityNameAsync_DaysValidationFail_ErrorMessage()
        {
            _cityNameValidator.Setup(v => v.Validate(It.IsAny<string>())).Returns(true);
            _forecastDaysValidator.Setup(v => v.Validate(It.IsAny<int>())).Returns(false);

            var forecastDescription = await _weatherService.GetForecastDescriptionByCityNameAsync("London", 1);

            Assert.That(forecastDescription, Is.EqualTo("Error: forecast days are not valid."));
        }

        [Test]
        public async Task GetForecastDescriptionByCityNameAsync_GetCoordinatesFail_ErrorMessage()
        {
            _cityNameValidator.Setup(v => v.Validate(It.IsAny<string>())).Returns(true);
            _forecastDaysValidator.Setup(v => v.Validate(It.IsAny<int>())).Returns(true);
            _geocodingRepository.Setup(g => g.GetCoordinatesByCityNameAsync(It.IsAny<string>())).ThrowsAsync(new JsonException("error"));

            var forecastDescription = await _weatherService.GetForecastDescriptionByCityNameAsync("London", 1);

            Assert.That(forecastDescription, Is.EqualTo("Error: failed to get weather forecast data (error)."));
        }

        [Test]
        public async Task GetForecastDescriptionByCityNameAsync_GetForecastSuccess_ForecastDescription()
        {
            string cityName = "London";
            var forecasts = new List<WeatherForecast>
            {
                new WeatherForecast(new DateTime(2023, 1, 25), 7, 9)
            };

            _cityNameValidator.Setup(v => v.Validate(It.IsAny<string>())).Returns(true);
            _forecastDaysValidator.Setup(v => v.Validate(It.IsAny<int>())).Returns(true);
            _geocodingRepository.Setup(g => g.GetCoordinatesByCityNameAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<Coordinates>());
            _weatherRepository.Setup(w => w.GetForecastByCoordinatesAsync(It.IsAny<Coordinates>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(forecasts);

            var forecastDescription = await _weatherService.GetForecastDescriptionByCityNameAsync(cityName, 1);

            Assert.That(forecastDescription, Is.EqualTo($"{cityName} weather forecast:{Environment.NewLine}Day 1: 8 °C. It's fresh."));
        }
    }
}
