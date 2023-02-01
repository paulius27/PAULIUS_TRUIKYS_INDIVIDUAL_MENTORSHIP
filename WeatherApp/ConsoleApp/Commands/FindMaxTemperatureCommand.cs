using BL;

namespace ConsoleApp.Commands;

internal class FindMaxTemperatureCommand : ICommand
{
    private IWeatherService _weatherService;

    public FindMaxTemperatureCommand(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    public async Task Execute()
    {
        Console.Write("Enter city names: ");
        var cityNamesInput = Console.ReadLine() ?? "";

        var cityNames = cityNamesInput
            .Split(',')
            .Select(cityName => cityName.Trim())
            .ToList();

        var result = await _weatherService.GetMaxTemperatureByCityNamesAsync(cityNames);
        Console.WriteLine(result);
    }
}
