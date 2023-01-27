using Microsoft.Extensions.Configuration;
using System;

namespace BL.Validation
{
    public class Validation : IValidation
    {
        private readonly IConfiguration _config;

        private int _forecastMinDays, _forecastMaxDays;

        public Validation(IConfiguration config)
        {
            _config = config;

            if (!int.TryParse(_config["WeatherForecast:MinDays"], out _forecastMinDays))
                _forecastMinDays = 1;

            if(!int.TryParse(_config["WeatherForecast:MaxDays"], out _forecastMaxDays))
                _forecastMaxDays = 7;
        }

        public bool IsCityNameValid(string cityName) => !string.IsNullOrEmpty(cityName?.Trim());

        public bool AreForecastDaysValid(int days) => days >= _forecastMinDays && days <= _forecastMaxDays;
    }
}
