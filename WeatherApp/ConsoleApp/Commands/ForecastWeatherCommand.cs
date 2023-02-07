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

        try
        {
            var forecast = await _weatherService.GetForecastByCityNameAsync(cityName, days);

            var i = 0;
            Console.Write($"{cityName} weather forecast:");

            foreach (var forecastDay in forecast.Days)
            {
                i++;
                Console.WriteLine();
                Console.Write($"Day {i}: {forecastDay.Temperature} °C. {forecastDay.Comment}.");
            }

            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}.");
        }
    }
}
