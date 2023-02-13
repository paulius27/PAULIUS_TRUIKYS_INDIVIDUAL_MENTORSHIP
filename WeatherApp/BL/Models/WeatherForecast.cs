using System.Collections.Generic;

namespace BL.Models
{
    public class WeatherForecast
    {
        public WeatherForecast(string cityName, List<WeatherForecastDay> days)
        {
            CityName = cityName;
            Days = days;
        }

        public string CityName { get; set; }

        public List<WeatherForecastDay> Days { get; set; }
    }
}
