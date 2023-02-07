using BL;

namespace ConsoleApp.Commands;

public class CurrentWeatherCommand : ICommand
{
    private IWeatherService _weatherService;

    public CurrentWeatherCommand(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    public async Task Execute()
    {
        Console.Write("Enter city name: ");
        var cityName = Console.ReadLine() ?? "";

        var weather = await _weatherService.GetWeatherByCityNameAsync(cityName);
        Console.WriteLine($"In {weather.CityName} {weather.Temperature} °C. {weather.Comment}.");
    }
}