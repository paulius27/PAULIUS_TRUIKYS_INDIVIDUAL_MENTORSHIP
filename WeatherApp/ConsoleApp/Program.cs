using BL;
using BL.Validation;
using ConsoleApp.Commands;
using DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHttpClientFactory httpClientFactory = null;
var host = new HostBuilder()
    .ConfigureServices((hostContext, services) =>
    {
        httpClientFactory = services.AddHttpClient().BuildServiceProvider().GetService<IHttpClientFactory>();
    })
    .UseConsoleLifetime()
    .Build();

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .AddJsonFile("appsettings.json")
    .Build();

string apiKey = config["weather_api_key"] ?? throw new KeyNotFoundException("Weather API Key not found.");

IValidator<string> cityNameValidator = new CityNameValidator();
IValidator<int> forecastDaysValidator = new ForecastDaysValidator(config);
IGeocodingRepository geocodingRepository = new GeocodingRepository(httpClientFactory, apiKey);
IWeatherRepository weatherRepository = new WeatherRepository(httpClientFactory, apiKey);
IWeatherService weatherService = new WeatherService(config, geocodingRepository, weatherRepository, cityNameValidator, forecastDaysValidator);

while (true)
{
    try
    {
        Console.WriteLine("1. Current weather");
        Console.WriteLine("2. Weather forecast");
        Console.WriteLine("3. Find max temperature");
        Console.WriteLine("0. Close application");

        Console.Write("Input: ");
        var input = Console.ReadKey().KeyChar;
        Console.WriteLine();

        ICommand command = input switch
        {
            '0' => new CloseApplicationCommand(),
            '1' => new CurrentWeatherCommand(weatherService),
            '2' => new ForecastWeatherCommand(weatherService),
            '3' => new FindMaxTemperatureCommand(weatherService),
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