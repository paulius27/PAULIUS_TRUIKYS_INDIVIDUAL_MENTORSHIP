using System;

namespace DAL.Models
{
    public class WeatherForecast
    {
        public WeatherForecast(DateTime date, double minTemperature, double maxTemperature)
        {
            Date = date;
            MinTemperature = minTemperature;
            MaxTemperature = maxTemperature;
        }

        public DateTime Date { get; set; }
        public double MinTemperature { get; set; }
        public double MaxTemperature { get; set; }
    }
}
