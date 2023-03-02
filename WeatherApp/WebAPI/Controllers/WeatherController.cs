using BL;
using BL.Models;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Responses;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;
    private readonly IWeatherHistoryService _weatherHistoryService;

    public WeatherController(IWeatherService weatherService, IWeatherHistoryService weatherHistoryService)
    {
        _weatherService = weatherService;
        _weatherHistoryService = weatherHistoryService;
    }

    [HttpGet("Current")]
    [ProducesResponseType(typeof(Weather), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCurrentWeather(string cityName)
    {
        var weather = await _weatherService.GetWeatherByCityNameAsync(cityName);
        return Ok(weather);
    }

    [HttpGet("Forecast")]
    [ProducesResponseType(typeof(WeatherForecast), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetWeatherForecast(string cityName, int days)
    {
        var forecast = await _weatherService.GetForecastByCityNameAsync(cityName, days);
        return Ok(forecast);
    }

    [HttpGet("History")]
    [ProducesResponseType(typeof(WeatherHistory), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetWeatherHistory(string cityName, DateTime from, DateTime to)
    {
        var history = await _weatherHistoryService.GetWeatherHistory(cityName, new TimeRange(from, to));
        return Ok(history);
    }
}