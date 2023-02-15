using Microsoft.Extensions.Options;
using Quartz;
using WebAPI.Jobs;
using WebAPI.Options;

namespace WebAPI
{
    public class Scheduler : IHostedService
    {
        private readonly IOptionsMonitor<WeatherHistoryOptions> _weatherHistoryOptions;
        private readonly ISchedulerFactory _schedulerFactory;
        private IScheduler _scheduler;

        public Scheduler(IOptionsMonitor<WeatherHistoryOptions> weatherHistoryOptions, ISchedulerFactory schedulerFactory) 
        {
            _weatherHistoryOptions = weatherHistoryOptions;
            _weatherHistoryOptions.OnChange(async (options) =>
            {
                await _scheduler.Clear();
                await ScheduleJobs();
            });

            _schedulerFactory = schedulerFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            await ScheduleJobs();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _scheduler.Clear(cancellationToken);
        }

        private async Task ScheduleJobs()
        {
            var cityGroups = _weatherHistoryOptions.CurrentValue.Cities.GroupBy(c => c.TimerSec).ToList();

            foreach (var cityGroup in cityGroups)
            {
                int timerSec = cityGroup.Key;
                var cityNames = cityGroup.Select(c => c.Name).ToList();
                var cityNamesString = string.Join(",", cityNames);

                var job = JobBuilder.Create<UpdateWeatherHistoryJob>()
                    .WithIdentity($"{cityNamesString}Job", "WeatherHistoryJobs")
                    .Build();

                job.JobDataMap["cityNames"] = cityNames;

                var trigger = TriggerBuilder.Create()
                    .WithIdentity($"{cityNamesString}Trigger", "WeatherHistoryTriggers")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(timerSec)
                        .RepeatForever())
                    .Build();

                await _scheduler.ScheduleJob(job, trigger);
            }
        }
    }
}
