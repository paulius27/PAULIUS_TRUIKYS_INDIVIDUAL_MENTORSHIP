using BL;
using BL.Validation;
using ConsoleApp.Commands;
using DAL;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

string apiKey = config["weather_api_key"] ?? throw new KeyNotFoundException("Weather API Key not found.");

IValidation validationService = new Validation();
IGeocodingRepository geocodingRepository = new GeocodingRepository(apiKey);
IWeatherRepository weatherRepository = new WeatherRepository(apiKey);
IWeatherService weatherService = new WeatherService(weatherRepository, validationService);

while (true)
{
    try
    {
        Console.WriteLine("0. Close application");
        Console.WriteLine("1. Current weather ");
        Console.WriteLine("2. Weather forecast");

        Console.Write("Input: ");
        var input = Console.ReadKey().KeyChar;
        Console.WriteLine();

        ICommand command = input switch
        {
            '0' => new CloseApplicationCommand(),
            '1' => new CurrentWeatherCommand(weatherService),
            '2' => new ForecastWeatherCommand(weatherService),
            _   => throw new ArgumentException($"Input \"{input}\" is not supported.")
        };

        await command.Execute();

        Console.WriteLine();
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine(ex.Message);
        Console.WriteLine();
    }
}