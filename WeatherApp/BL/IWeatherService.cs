using System;
using System.Threading.Tasks;

namespace BL
{
    public interface IWeatherService
    {
        public Task<string> GetWeatherDescriptionByCityNameAsync(string cityName);
    }
}
