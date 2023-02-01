using Microsoft.Extensions.Configuration;

namespace BL.Validation
{
    public class ForecastDaysValidator : IValidator<int>
    {
        private int _forecastMinDays, _forecastMaxDays;

        public ForecastDaysValidator(IConfiguration config)
        {
            if (!int.TryParse(config["WeatherForecast:MinDays"], out _forecastMinDays))
                _forecastMinDays = 1;

            if (!int.TryParse(config["WeatherForecast:MaxDays"], out _forecastMaxDays))
                _forecastMaxDays = 7;
        }

        public bool Validate(int days) => days >= _forecastMinDays && days <= _forecastMaxDays;
    }
}