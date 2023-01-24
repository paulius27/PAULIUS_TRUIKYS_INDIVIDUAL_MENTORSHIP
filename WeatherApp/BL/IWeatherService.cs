using System.Threading.Tasks;

namespace BL
{
    public interface IWeatherService
    {
        public Task<string> GetWeatherDescriptionByCityNameAsync(string cityName);

        public Task<string> GetForecastDescriptionByCityNameAsync(string cityName, int days);
    }
}
