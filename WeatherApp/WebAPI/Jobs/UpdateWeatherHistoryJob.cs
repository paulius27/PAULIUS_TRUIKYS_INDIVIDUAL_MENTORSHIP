using BL;
using Quartz;
using WebAPI.Scheduler;

namespace WebAPI.Jobs
{
    public class UpdateWeatherHistoryJob : IJob
    {
        private readonly IWeatherHistoryService _weatherHistoryService;

        public UpdateWeatherHistoryJob(IWeatherHistoryService weatherHistoryRepository)
        {
            _weatherHistoryService = weatherHistoryRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var cityNames = (string[])context.MergedJobDataMap[SchedulerJobDataConstants.CityNames];

            if (cityNames == null)
                return;

            await _weatherHistoryService.UpdateWeatherHistory(cityNames);
        }
    }
}
