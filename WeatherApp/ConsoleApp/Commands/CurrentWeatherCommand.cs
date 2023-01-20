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

        var weatherDescription = await _weatherService.GetWeatherDescriptionByCityNameAsync(cityName);
        Console.WriteLine(weatherDescription);
    }
}