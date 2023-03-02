using BL;
using BL.Models;
using BL.Validation;
using DAL;
using DAL.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests
{
    public class WeatherHistoryServiceTest
    {
        private Mock<ILogger<WeatherHistoryService>> _logger;
        private Mock<ICityService> _cityService;
        private Mock<IWeatherHistoryRepository> _weatherHistoryRepository;
        private Mock<IWeatherService> _weatherService;
        private Mock<IValidator<TimeRange>> _timeRangeValidator;

        private WeatherHistoryService _weatherHistoryService;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<WeatherHistoryService>>();
            _cityService = new Mock<ICityService>();
            _weatherHistoryRepository = new Mock<IWeatherHistoryRepository>();
            _weatherService = new Mock<IWeatherService>();
            _timeRangeValidator = new Mock<IValidator<TimeRange>>();

            _weatherHistoryService = new WeatherHistoryService(_logger.Object, _cityService.Object, _weatherHistoryRepository.Object, _weatherService.Object, _timeRangeValidator.Object);
        }

        [Test]
        public void GetWeatherHistory_TimeRangeValidationFail_Error() 
        {
            _timeRangeValidator.Setup(v => v.Validate(It.IsAny<TimeRange>())).Returns(false);

            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _weatherHistoryService.GetWeatherHistory("Paris", new TimeRange(new DateTime(2023, 1, 1), new DateTime(2022, 1, 1)));
            });

            Assert.That(ex.Message, Is.EqualTo("time range is not valid"));
        }

        [Test]
        public void GetWeatherHistory_FindCityFail_Error()
        {
            _timeRangeValidator.Setup(v => v.Validate(It.IsAny<TimeRange>())).Returns(true);
            _cityService.Setup(c => c.FindCity(It.IsAny<string>())).ReturnsAsync((City?)null);

            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await _weatherHistoryService.GetWeatherHistory("?", new TimeRange(new DateTime(2023, 1, 1), new DateTime(2023, 2, 1)));
            });

            Assert.That(ex.Message, Is.EqualTo("city not found"));
        }

        [Test]
        public async Task GetWeatherHistory_FindHistorySuccess_WeatherHistory()
        {
            var temperature = 10;
            var time = new DateTime(2023, 2, 2);
            var weatherHistoryEntry = new WeatherHistoryEntry { Id = 1, Temperature = temperature, Time = time, CityId =  1 };
            var weatherHistoryData = new List<WeatherHistoryEntry>() { weatherHistoryEntry };
            var city = new City { Id = 1, Name = "Berlin" };

            _timeRangeValidator.Setup(v => v.Validate(It.IsAny<TimeRange>())).Returns(true);
            _cityService.Setup(c => c.FindCity(It.IsAny<string>())).ReturnsAsync(city);
            _weatherHistoryRepository.Setup(w => w.FindByCityIdAndTimeRange(It.IsAny<int>(), It.IsAny<TimeRange>())).ReturnsAsync(weatherHistoryData);

            var weatherHistory = await _weatherHistoryService.GetWeatherHistory(city.Name, new TimeRange(new DateTime(2023, 2, 1), new DateTime(2023, 2, 3)));

            Assert.That(weatherHistory.CityName, Is.EqualTo(city.Name));
            Assert.That(weatherHistory.Data.Count, Is.EqualTo(1));
            Assert.That(weatherHistory.Data.First().Temperature, Is.EqualTo(temperature));
            Assert.That(weatherHistory.Data.First().Time, Is.EqualTo(time));
        }
    }
}
