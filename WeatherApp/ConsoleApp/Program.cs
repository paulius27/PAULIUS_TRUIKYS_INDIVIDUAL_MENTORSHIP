using BL;
using BL.Validation;
using DAL;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

string apiKey = config["weather_api_key"] ?? throw new KeyNotFoundException("Weather API Key not found.");

IValidation validationService = new Validation();
IWeatherRepository weatherRepository = new WeatherRepository(apiKey);
IWeatherService weatherService = new WeatherService(weatherRepository, validationService);

while (true)
{
    Console.Write("Enter city name: ");
    var cityName = Console.ReadLine() ?? "";

    var weatherDescription = await weatherService.GetWeatherDescriptionByCityNameAsync(cityName);
    Console.WriteLine(weatherDescription);
}