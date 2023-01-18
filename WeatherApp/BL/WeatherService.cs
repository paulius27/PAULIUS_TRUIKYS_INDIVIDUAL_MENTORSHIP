using System;
using System.Threading.Tasks;
using BL.Validation;
using DAL;

namespace BL
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherRepository _weatherRepository;
        private readonly IValidation _validation;

        public WeatherService(IWeatherRepository weatherRepository, IValidation validationService)
        {
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
