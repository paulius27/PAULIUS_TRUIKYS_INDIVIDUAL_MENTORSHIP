using Microsoft.Extensions.Configuration;

namespace BL.Validation
{
    public class Validation : IValidation
    {
        private readonly IConfiguration _config;

        private int _forecastMinDays, _forecastMaxDays;

        public Validation(IConfiguration config)
        {
            _config = config;
            _forecastMinDays = int.Parse(_config["WeatherForecast:MinDays"]);
            _forecastMaxDays = int.Parse(_config["WeatherForecast:MaxDays"]);
        }

        public bool IsCityNameValid(string cityName) => !string.IsNullOrEmpty(cityName?.Trim());

        public bool AreForecastDaysValid(int days) => days >= _forecastMinDays && days <= _forecastMaxDays;
    }
}
