using Quartz;

namespace WebAPI.Jobs
{
    public class UpdateWeatherHistoryJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var cityNames = (IEnumerable<string>)context.MergedJobDataMap["cityNames"];

            await Console.Out.WriteLineAsync($"UpdateWeatherHistoryJob: {string.Join("-", cityNames)}");
        }
    }
}
