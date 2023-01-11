using System.Threading.Tasks;
using DAL;

namespace BL
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherRepository _weatherRepository;

        public WeatherService(IWeatherRepository weatherRepository)
        {
            _weatherRepository = weatherRepository;
        }

        public async Task<string> GetWeatherDescriptionByCityNameAsync(string cityName)
        {
            double temperature = await _weatherRepository.GetTemperatureByCityNameAsync(cityName);
            return $"In {cityName} {temperature} °C. {GetTemperatureComment(temperature)}.";
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
