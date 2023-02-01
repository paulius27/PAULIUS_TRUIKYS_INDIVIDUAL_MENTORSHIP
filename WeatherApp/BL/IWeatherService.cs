using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL
{
    public interface IWeatherService
    {
        Task<string> GetWeatherDescriptionByCityNameAsync(string cityName);

        Task<string> GetForecastDescriptionByCityNameAsync(string cityName, int days);

        Task<string> GetMaxTemperatureByCityNamesAsync(IEnumerable<string> cityNames);
    }
}
