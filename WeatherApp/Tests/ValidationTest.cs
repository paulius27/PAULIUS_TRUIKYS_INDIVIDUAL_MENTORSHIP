using BL.Validation;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Tests
{
    public class ValidationTest
    {
        private Mock<IConfiguration> _config;
        private IValidation _validation;

        [SetUp]
        public void Setup()
        {
            _config = new Mock<IConfiguration>();
            _config.SetupGet(c => c["WeatherForecast:MinDays"]).Returns("1");
            _config.SetupGet(c => c["WeatherForecast:MaxDays"]).Returns("7");

            _validation = new Validation(_config.Object);
        }

        [Test]
        public void IsCityNameValid_ValidName_True()
        {
            var cityName = "London";

            var validationResult = _validation.IsCityNameValid(cityName);

            Assert.IsTrue(validationResult);
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void IsCityNameValid_InvalidName_False(string cityName)
        {
            var validationResult = _validation.IsCityNameValid(cityName);

            Assert.IsFalse(validationResult);
        }

        [Test]
        public void AreForecastDaysValid_ValidDays_True()
        {
            var days = 5;

            var validationResult = _validation.AreForecastDaysValid(days);

            Assert.IsTrue(validationResult);
        }

        [Test]
        [TestCase(0)]
        [TestCase(10)]
        public void AreForecastDaysValid_InvalidDays_False(int days)
        {
            var validationResult = _validation.AreForecastDaysValid(days);

            Assert.IsFalse(validationResult);
        }
    }
}