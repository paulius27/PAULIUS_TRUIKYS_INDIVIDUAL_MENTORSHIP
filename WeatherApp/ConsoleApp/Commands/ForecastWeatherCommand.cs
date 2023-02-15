using BL;
using System.Text;

namespace ConsoleApp.Commands;

public class ForecastWeatherCommand : ICommand<string>
{
    private IWeatherService _weatherService;
    private string _cityName;
    private int _days;

    public ForecastWeatherCommand(IWeatherService weatherService, string cityName, int days)
    {
        _weatherService = weatherService;
        _cityName = cityName;
        _days = days;
    }

    public async Task<string> Execute()
    {
        try
        {
            var forecast = await _weatherService.GetForecastByCityNameAsync(_cityName, _days);

            var i = 0;
            var sb = new StringBuilder($"{_cityName} weather forecast:");

            foreach (var forecastDay in forecast.Days)
            {
                i++;
                sb.AppendLine();
                sb.Append($"Day {i}: {forecastDay.Temperature} °C. {forecastDay.Comment}.");
            }

            return sb.ToString();
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}.";
        }
    }
}
