using DAL;
using DAL.Models;
using Quartz;

namespace WebAPI.Jobs
{
    public class UpdateWeatherHistoryJob : IJob
    {
        private readonly IWeatherHistoryRepository _weatherHistoryRepository;

        public UpdateWeatherHistoryJob(IWeatherHistoryRepository weatherHistoryRepository)
        {
            _weatherHistoryRepository = weatherHistoryRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var cityNames = (IEnumerable<string>)context.MergedJobDataMap["cityNames"];
            await Console.Out.WriteLineAsync($"UpdateWeatherHistoryJob: {string.Join("-", cityNames)}");

            var weatherHistoryEntry1 = new WeatherHistoryEntry
            {
                CityName = "Paris",
                Temperature = 10,
                Time = DateTime.Now
            };

            var weatherHistoryEntry2 = new WeatherHistoryEntry
            {
                CityName = "Miami",
                Temperature = 20,
                Time = DateTime.Now
            };

            var data = new List<WeatherHistoryEntry> { weatherHistoryEntry1, weatherHistoryEntry2 };

            await _weatherHistoryRepository.InsertMany(data);
        }
    }
}
