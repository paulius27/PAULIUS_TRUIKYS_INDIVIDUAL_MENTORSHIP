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
        public void IsCityNameValid_NullName_False()
        {
            string cityName = null;

            var validationResult = _validation.IsCityNameValid(cityName);

            Assert.IsFalse(validationResult);
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        public void IsCityNameValid_EmptyName_False(string cityName)
        {
            var validationResult = _validation.IsCityNameValid(cityName);

            Assert.IsFalse(validationResult);
        }
    }
}