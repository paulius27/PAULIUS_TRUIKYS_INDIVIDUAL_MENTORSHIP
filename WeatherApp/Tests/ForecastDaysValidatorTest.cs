using BL.Validation;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Tests
{
    public class ForecastDaysValidatorTest
    {
        private Mock<IConfiguration> _config;
        private IValidator<int> _forecastDaysValidator;

        [SetUp]
        public void Setup()
        {
            _config = new Mock<IConfiguration>();
            _config.SetupGet(c => c["WeatherForecast:MinDays"]).Returns("1");
            _config.SetupGet(c => c["WeatherForecast:MaxDays"]).Returns("7");

            _forecastDaysValidator = new ForecastDaysValidator(_config.Object);
        }

        [Test]
        public void Validate_ValidForecastDays_True()
        {
            var days = 5;

            var validationResult = _forecastDaysValidator.Validate(days);

            Assert.That(validationResult, Is.True);
        }

        [Test]
        [TestCase(0)]
        [TestCase(10)]
        public void Validate_InvalidForecastDays_False(int days)
        {
            var validationResult = _forecastDaysValidator.Validate(days);

            Assert.That(validationResult, Is.False);
        }
    }
}