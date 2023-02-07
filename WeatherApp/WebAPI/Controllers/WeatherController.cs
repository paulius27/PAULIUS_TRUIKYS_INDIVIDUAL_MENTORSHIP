using BL;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpGet("Current")]
    public async Task<IActionResult> GetCurrentWeather(string cityName)
    {
        var weather = await _weatherService.GetWeatherByCityNameAsync(cityName);
        return Ok(weather);
    }

    [HttpGet("Forecast")]
    public async Task<IActionResult> GetWeatherForecast(string cityName, int days)
    {
        var forecast = await _weatherService.GetForecastByCityNameAsync(cityName, days);
        return Ok(forecast);
    }
}