using BL;

namespace ConsoleApp.Commands;

public class ForecastWeatherCommand : ICommand
{
    private IWeatherService _weatherService;

    public ForecastWeatherCommand(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    public async Task Execute()
    {
        Console.Write("Enter city name: ");
        var cityName = Console.ReadLine() ?? "";

        Console.Write("Enter how many days to forecast: ");
        if (!int.TryParse(Console.ReadLine(), out int days)) 
        {
            Console.WriteLine("Input for 'days' must be a number.");
            return;
        }

        var forecastDescription = await _weatherService.GetForecastDescriptionByCityNameAsync(cityName, days);
        Console.Write(forecastDescription);
    }
}
