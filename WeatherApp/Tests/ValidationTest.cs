using BL.Validation;

namespace Tests
{
    public class ValidationTest
    {
        private IValidation _validation;

        [SetUp]
        public void Setup()
        {
            _validation = new Validation();
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
        [TestCase("x")]
        [TestCase(" ")]
        public void IsCityNameValid_EmptyName_False(string cityName)
        {
            var validationResult = _validation.IsCityNameValid(cityName);

            Assert.IsFalse(validationResult);
        }
    }
}