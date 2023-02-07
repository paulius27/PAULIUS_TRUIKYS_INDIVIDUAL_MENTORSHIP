using System.Collections.Generic;

namespace BL.Models
{
    public class WeatherForecast
    {
        public WeatherForecast(string cityName, IEnumerable<WeatherForecastDay> days)
        {
            CityName = cityName;
            Days = days;
        }

        public string CityName { get; set; }

        public IEnumerable<WeatherForecastDay> Days { get; set; }
    }
}
