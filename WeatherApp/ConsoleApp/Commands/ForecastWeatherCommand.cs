using BL;

namespace ConsoleApp.Commands;

public class ForecastWeatherCommand : ICommand
{
    private IWeatherService _weatherService;

    public ForecastWeatherCommand(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    public Task Execute()
    {
        throw new NotImplementedException();
    }
}
