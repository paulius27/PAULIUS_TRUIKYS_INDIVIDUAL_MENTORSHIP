using System;
using System.Text;
using System.Threading.Tasks;
using BL.Validation;
using DAL;

namespace BL
{
    public class WeatherService : IWeatherService
    {
        private readonly IGeocodingRepository _geocodingRepository;
        private readonly IWeatherRepository _weatherRepository;
        private readonly IValidation _validation;

        public WeatherService(IGeocodingRepository geocodingRepository, IWeatherRepository weatherRepository, IValidation validationService)
        {
            _geocodingRepository = geocodingRepository;
            _weatherRepository = weatherRepository;
            _validation = validationService;
        }

        public async Task<string> GetWeatherDescriptionByCityNameAsync(string cityName)
        {
            if (!_validation.IsCityNameValid(cityName))
                return "Error: city name is not valid.";
            
            try
            {
                double temperature = await _weatherRepository.GetTemperatureByCityNameAsync(cityName);
                return $"In {cityName} {temperature} °C. {GetTemperatureComment(temperature)}.";
            }
            catch (Exception ex)
            {
                return $"Error: failed to get weather data ({ex.Message}).";
            }
        }

        public async Task<string> GetForecastDescriptionByCityNameAsync(string cityName, int days)
        {
            if (!_validation.IsCityNameValid(cityName))
                return "Error: city name is not valid.";

            if (!_validation.AreForecastDaysValid(days))
                return "Error: forecast days are not valid.";

            try
            {
                var coordinates = await _geocodingRepository.GetCoordinatesByCityNameAsync(cityName);

                DateTime startDate = DateTime.Now.AddDays(1);
                DateTime endDate = startDate.AddDays(days - 1);
                var forecasts = await _weatherRepository.GetForecastByCoordinatesAsync(coordinates, startDate, endDate);

                var i = 0;
                var sb = new StringBuilder();
                sb.AppendLine($"{cityName} weather forecast:");

                foreach (var forecast in forecasts)
                {
                    i++;
                    double temperature = Math.Round((forecast.MinTemperature + forecast.MaxTemperature) / 2, 2);
                    sb.AppendLine($"Day {i}: {temperature} °C. {GetTemperatureComment(temperature)}.");
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                return $"Error: failed to get weather forecast data ({ex.Message}).";
            }
        }

        private string GetTemperatureComment(double temperature)
        {
            if (temperature < 0)
                return "Dress warmly";
            else if (temperature < 20)
                return "It's fresh";
            else if (temperature < 30)
                return "Good weather";
            else
                return "It's time to go to the beach";
        }
    }
}
