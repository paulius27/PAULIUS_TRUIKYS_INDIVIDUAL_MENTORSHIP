using BL;
using BL.Validation;
using DAL;
using Moq;
using Newtonsoft.Json;

namespace Tests
{
    public class WeatherServiceTest
    {
        private Mock<IWeatherRepository> _weatherRepository;
        private Mock<IValidation> _validation;

        private WeatherService _weatherService;

        [SetUp]
        public void Setup()
        {
            _weatherRepository= new Mock<IWeatherRepository>();
            _validation = new Mock<IValidation>();

            _weatherService = new WeatherService(_weatherRepository.Object, _validation.Object);
        }

        [Test]
        public async Task GetWeatherDescriptionByCityNameAsync_ValidationFail_ErrorMessage()
        {
            _validation.Setup(v => v.IsCityNameValid(It.IsAny<string>())).Returns(false);

            var weatherDescription = await _weatherService.GetWeatherDescriptionByCityNameAsync("");

            Assert.That(weatherDescription, Is.EqualTo("Error: city name is not valid."));
        }

        [Test]
        public async Task GetWeatherDescriptionByCityNameAsync_GetTemperatureFail_ErrorMessage()
        {
            _validation.Setup(v => v.IsCityNameValid(It.IsAny<string>())).Returns(true);
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
            _validation.Setup(v => v.IsCityNameValid(It.IsAny<string>())).Returns(true);
            _weatherRepository.Setup(w => w.GetTemperatureByCityNameAsync(It.IsAny<string>())).ReturnsAsync(temperature);

            var weatherDescription = await _weatherService.GetWeatherDescriptionByCityNameAsync(cityName);

            Assert.That(weatherDescription, Is.EqualTo($"In {cityName} {temperature} °C. {expectedTemperatureComment}."));
        }
    }
}
