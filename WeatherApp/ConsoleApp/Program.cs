using BL;
using DAL;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

string apiKey = config["weather_api_key"] ?? throw new KeyNotFoundException("Weather API Key not found.");

IWeatherRepository weatherRepository = new WeatherRepository(apiKey);
IWeatherService weatherService = new WeatherService(weatherRepository);

while (true)
{
    Console.Write("Enter city name: ");
    var cityName = Console.ReadLine() ?? "";

    var weatherDescription = await weatherService.GetWeatherDescriptionByCityNameAsync(cityName);
    Console.WriteLine(weatherDescription);
}