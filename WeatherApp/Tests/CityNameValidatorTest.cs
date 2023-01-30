using BL.Validation;

namespace Tests
{
    internal class CityNameValidatorTest
    {
        private IValidator<string> _cityNameValidator;

        [SetUp]
        public void Setup()
        {
            _cityNameValidator = new CityNameValidator();
        }

        [Test]
        public void Validate_ValidCityName_True()
        {
            var cityName = "London";

            var validationResult = _cityNameValidator.Validate(cityName);

            Assert.That(validationResult, Is.True);
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void Validate_InvalidCityName_False(string cityName)
        {
            var validationResult = _cityNameValidator.Validate(cityName);

            Assert.That(validationResult, Is.False);
        }
    }
}
