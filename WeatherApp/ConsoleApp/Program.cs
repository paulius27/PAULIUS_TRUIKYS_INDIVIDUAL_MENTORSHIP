using BL;
using DAL;

IWeatherRepository weatherRepository = new WeatherRepository();
IWeatherService weatherService = new WeatherService(weatherRepository);

while (true)
{
    Console.Write("Enter city name: ");
    var cityName = Console.ReadLine() ?? "";

    var weatherDescription = await weatherService.GetWeatherDescriptionByCityNameAsync(cityName);
    Console.WriteLine(weatherDescription);
}