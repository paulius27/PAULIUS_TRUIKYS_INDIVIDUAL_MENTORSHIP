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

IValidator<string> cityNameValidator = new CityNameValidator();
IValidator<int> forecastDaysValidator = new ForecastDaysValidator(config);
IGeocodingRepository geocodingRepository = new GeocodingRepository(config, httpClientFactory);
IWeatherRepository weatherRepository = new WeatherRepository(config, httpClientFactory);
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

        ICommand<string> command;

        if (input == '1')
        {
            Console.Write("Enter city name: ");
            var cityName = Console.ReadLine() ?? "";

            command = new CurrentWeatherCommand(weatherService, cityName);
        }
        else if (input == '2')
        {
            Console.Write("Enter city name: ");
            var cityName = Console.ReadLine() ?? "";

            Console.Write("Enter how many days to forecast: ");
            if (!int.TryParse(Console.ReadLine(), out int days))
                throw new ArgumentException("Input for 'days' must be a number.");

            command = new ForecastWeatherCommand(weatherService, cityName, days);
        }
        else if (input == '3')
        {
            Console.Write("Enter city names: ");
            var cityNames = Console.ReadLine() ?? "";

            command = new FindMaxTemperatureCommand(weatherService, cityNames);
        }
        else if (input == '0')
            break;
        else
            throw new ArgumentException($"Input \"{input}\" is not supported.");

        var result = await command.Execute();
        Console.WriteLine(result);
        Console.WriteLine();
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine(ex.Message);
        Console.WriteLine();
    }
}