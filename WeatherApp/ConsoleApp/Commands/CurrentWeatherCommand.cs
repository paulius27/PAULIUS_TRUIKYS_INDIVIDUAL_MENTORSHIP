using BL;

namespace ConsoleApp.Commands;

public class CurrentWeatherCommand : ICommand<string>
{
    private IWeatherService _weatherService;

    private string _cityName;

    public CurrentWeatherCommand(IWeatherService weatherService, string cityName)
    {
        _weatherService = weatherService;
        _cityName = cityName;
    }

    public async Task<string> Execute()
    {
        try
        {
            var weather = await _weatherService.GetWeatherByCityNameAsync(_cityName);
            return $"In {weather.CityName} {weather.Temperature} °C. {weather.Comment}.";
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}.";
        }
    }
}