using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherController : ControllerBase
{
    private readonly ILogger<WeatherController> _logger;

    public WeatherController(ILogger<WeatherController> logger)
    {
        _logger = logger;
    }

    [HttpGet("Current")]
    public async Task<IActionResult> GetCurrentWeather(string cityName)
    {
        await Task.Delay(100);
        return Ok("Current Weather");
    }

    [HttpGet("Forecast")]
    public async Task<IActionResult> GetWeatherForecast(string cityName, int days)
    {
        await Task.Delay(100);
        return Ok("Weather Forecast");
    }
}