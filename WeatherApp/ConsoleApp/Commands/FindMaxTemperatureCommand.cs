using BL;

namespace ConsoleApp.Commands;

internal class FindMaxTemperatureCommand : ICommand
{
    private IWeatherService _weatherService;

    private string _cityNames;

    public FindMaxTemperatureCommand(IWeatherService weatherService, string cityNames)
    {
        _weatherService = weatherService;
        _cityNames = cityNames;
    }

    public async Task<string> Execute()
    {
        var cityNamesList = _cityNames
            .Split(',')
            .Select(cityName => cityName.Trim())
            .ToList();

        return await _weatherService.GetMaxTemperatureByCityNamesAsync(cityNamesList);
    }
}
